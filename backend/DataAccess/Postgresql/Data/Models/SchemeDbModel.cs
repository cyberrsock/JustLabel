using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustLabel.Data.Models;

[Table("Schemes")]
public class SchemeDbModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [ForeignKey("User")]
    [Column("creatorID")]
    public int CreatorId { get; set; }

    [Column("createDatetime", TypeName = "timestamp")]
    public DateTime CreateDatetime { get; set; }

    public UserDbModel? User { get; set; }

    public ICollection<MarkedDbModel> Marks {get;set;} = [];
    public ICollection<LabelDbModel> Labels {get;set;} = [];
    public ICollection<LabelSchemeDbModel> LabelsSchemes {get;set;} = [];

}
