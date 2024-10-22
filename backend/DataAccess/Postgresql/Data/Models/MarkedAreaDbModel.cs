using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustLabel.Data.Models;

[Table("MarksAreas")]
public class MarkedAreaDbModel
{
    [ForeignKey("Mark")]
    [Column("markedID")]
    public int MarkedId { get; set; }

    [ForeignKey("Area")]
    [Column("areaID")]
    public int AreaId { get; set; }

    public MarkedDbModel? Mark { get; set; }
    public AreaDbModel? Area { get; set; }

}
