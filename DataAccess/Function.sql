CREATE OR REPLACE FUNCTION iou(x1_1 INTEGER, y1_1 INTEGER, x2_1 INTEGER, y2_1 INTEGER, x1_2 INTEGER, y1_2 INTEGER, x2_2 INTEGER, y2_2 INTEGER, entry_koef FLOAT)
RETURNS FLOAT AS
$$
DECLARE
    intersection_area INTEGER;
    union_area INTEGER;
    ratio FLOAT;
BEGIN
    intersection_area :=
        GREATEST(0, LEAST(x2_1, x2_2) - GREATEST(x1_1, x1_2)) * GREATEST(0, LEAST(y2_1, y2_2) - GREATEST(y1_1, y1_2));

	IF x1_1 >= x1_2 AND y1_1 >= y1_2 AND x2_1 <= x2_2 AND y2_1 <= y2_2 THEN
        union_area := ABS((x2_2 - x1_2) * (y2_2 - y1_2));
		intersection_area := intersection_area * entry_koef;
    ELSIF x1_2 >= x1_1 AND y1_2 >= y1_1 AND x2_2 <= x2_1 AND y2_2 <= y2_1 THEN
        union_area := ABS((x2_1 - x1_1) * (y2_1 - y1_1));
		intersection_area := intersection_area * entry_koef;
    ELSE
        union_area := ABS((x2_1 - x1_1) * (y2_1 - y1_1)) + ABS((x2_2 - x1_2) * (y2_2 - y1_2)) - intersection_area;
    END IF;
	
    IF union_area > 0 THEN
        ratio := intersection_area::FLOAT / union_area::FLOAT;
    ELSE
        ratio := 0.0;
    END IF;

    RETURN ratio;
END;
$$
LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION calc_aggregation(dataset_id INTEGER, scheme_id INTEGER, iou_min FLOAT, entry_koef FLOAT)
RETURNS TABLE (
    imageid INTEGER,
    labelid INTEGER,
    x1 INTEGER,
    y1 INTEGER,
    x2 INTEGER,
    y2 INTEGER
) AS $$
DECLARE
    current_class INTEGER := 1;
    rec_outer RECORD;
    rec_inner RECORD;
BEGIN
    DROP TABLE IF EXISTS temp_marked;
    CREATE TEMP TABLE temp_marked AS 
    SELECT a.id, m."imageID", "labelID", a.x1, a.x2, a.y1, a.y2
    FROM "Images" as i
    JOIN "Marks" AS m ON i.id = m."imageID"
    JOIN "MarksAreas" ma ON m.id = ma."markedID"
    JOIN "Areas" a ON ma."areaID" = a.id
    WHERE "datasetID" = dataset_id
	AND "schemeID" = scheme_id
    AND "isBlocked" = false;

    DROP TABLE IF EXISTS graph_table;
    CREATE TEMP TABLE graph_table AS 
    SELECT
        0 as class,
        t1.id as id1,
        t2.id as id2,
        t1."imageID",
        t1."labelID",
        t1.x1 as t1_x1,
        t1.x2 as t1_x2,
        t1.y1 as t1_y1,
        t1.y2 as t1_y2,
        t2.x1 as t2_x1,
        t2.x2 as t2_x2,
        t2.y1 as t2_y1,
        t2.y2 as t2_y2
    FROM temp_marked as t1
    CROSS JOIN temp_marked as t2
    WHERE t1.id < t2.id
    AND t1."imageID" = t2."imageID"
    AND t1."labelID" = t2."labelID"
    AND iou(t1.x1, t1.y1, t1.x2, t1.y2, t2.x1, t2.y1, t2.x2, t2.y2, entry_koef) > iou_min;

    FOR rec_outer IN SELECT id1, id2 FROM graph_table FOR UPDATE
    LOOP
        IF (SELECT g.class FROM graph_table g WHERE g.id1 = rec_outer.id1 AND g.id2 = rec_outer.id2 LIMIT 1) = 0 THEN
            UPDATE graph_table SET class = current_class WHERE id1 = rec_outer.id1 AND id2 = rec_outer.id2;
            
            FOR rec_inner IN SELECT id1, id2, h.class FROM graph_table h WHERE
			(h.id1 = rec_outer.id1 OR h.id1 = rec_outer.id2 OR
            h.id2 = rec_outer.id1 OR h.id2 = rec_outer.id2) AND
			h.class > 0 AND h.class < current_class
			FOR UPDATE
            LOOP
            	UPDATE graph_table SET class = rec_inner.class WHERE rec_outer.id1 = id1 AND rec_outer.id2 = id2;
				current_class := current_class - 1;
				EXIT;
            END LOOP;
			current_class := current_class + 1;
        END IF;
    END LOOP;

    RETURN QUERY
    SELECT
        g."imageID",
        g."labelID",
        CAST(AVG((t1_x1/2 + t2_x1/2)::NUMERIC) AS INTEGER),
        CAST(AVG((t1_y1/2 + t2_y1/2)::NUMERIC) AS INTEGER),
        CAST(AVG((t1_x2/2 + t2_x2/2)::NUMERIC) AS INTEGER),
        CAST(AVG((t1_y2/2 + t2_y2/2)::NUMERIC) AS INTEGER)
    FROM graph_table g
    GROUP BY g.class, g."labelID", g."imageID";

END;
$$ LANGUAGE plpgsql;

SELECT * FROM calc_aggregation(1, 1, 0.5, 2);
