using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustLabel.Data.Models;

[Table("Labels")]
public class LabelDbModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    public ICollection<AreaDbModel> Areas {get;set;} = [];
    public ICollection<SchemeDbModel> Schemes {get;set;} = [];
    public ICollection<LabelSchemeDbModel> LabelsSchemes {get;set;} = [];
}
