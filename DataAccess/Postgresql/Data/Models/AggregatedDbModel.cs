using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustLabel.Data.Models;

public class AggregatedDbModel
{
    [Key]
    [Column("imageid")]
    public int ImageId { get; set; }

    [Key]
    [Column("labelid")]
    public int LabelId { get; set; }

    [Column("x1")]
    public int X1 { get; set; }

    [Column("y1")]
    public int Y1 { get; set; }

    [Column("x2")]
    public int X2 { get; set; }

    [Column("y2")]
    public int Y2 { get; set; }
}
