using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustLabel.Data.Models;

[Table("Marks")]
public class MarkedDbModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey("Scheme")]
    [Column("schemeID")]
    public int SchemeId { get; set; }

    [ForeignKey("Image")]
    [Column("imageID")]
    public int ImageId { get; set; }

    [ForeignKey("User")]
    [Column("creatorID")]
    public int CreatorId { get; set; }

    [Column("isBlocked")]
    public bool IsBlocked { get; set; }

    [Column("loadDatetime", TypeName = "timestamp")]
    public DateTime CreateDatetime { get; set; }

    public SchemeDbModel? Scheme { get; set; }
    public ImageDbModel? Image { get; set; }
    public UserDbModel? User { get; set; }

    public ICollection<AreaDbModel> Areas {get;set;} = [];
    public ICollection<MarkedAreaDbModel> MarksAreas {get;set;} = [];
    public ICollection<ReportDbModel> Reports {get;set;} = [];

}
