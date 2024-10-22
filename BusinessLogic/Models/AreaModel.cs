namespace JustLabel.Models;

public class AreaModel
{
    public int Id { get; set; }

    public int LabelId { get; set; }

    public Point[] Coords { get; set; }
}

public class Point
{
    public double X { get; set; }
    public double Y { get; set; }
}
