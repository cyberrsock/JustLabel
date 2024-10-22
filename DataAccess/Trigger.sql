CREATE OR REPLACE FUNCTION update_marks_blocked()
RETURNS TRIGGER AS $$
BEGIN
	IF NEW."blockMarks" != OLD."blockMarks" THEN
		UPDATE "Marks"
		SET "isBlocked" = NEW."blockMarks"
		WHERE "creatorID" = NEW."id";
	END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_update_marks_blocked
AFTER UPDATE OF "blockMarks"
ON "Users"
FOR EACH ROW
EXECUTE FUNCTION update_marks_blocked();

DROP TRIGGER IF EXISTS trigger_update_marks_blocked ON "Users";
DROP FUNCTION IF EXISTS update_marks_blocked;