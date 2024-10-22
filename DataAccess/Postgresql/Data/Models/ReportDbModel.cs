using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustLabel.Data.Models;

[Table("Reports")]
public class ReportDbModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey("Marked")]
    [Column("markedID")]
    public int MarkedId { get; set; }

    [ForeignKey("User")]
    [Column("creatorID")]
    public int CreatorId { get; set; }

    [Column("comment")]
    public string Comment { get; set; }

    [Column("loadDatetime", TypeName = "timestamp")]
    public DateTime LoadDatetime { get; set; }

    public MarkedDbModel? Mark { get; set; }
    public UserDbModel? User { get; set; }
}
