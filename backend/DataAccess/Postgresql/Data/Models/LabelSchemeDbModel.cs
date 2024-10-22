using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustLabel.Data.Models;

[Table("LabelsSchemes")]
public class LabelSchemeDbModel
{
    [ForeignKey("Label")]
    [Column("labelID")]
    public int LabelId { get; set; }

    [ForeignKey("Scheme")]
    [Column("schemeID")]
    public int SchemeId { get; set; }

    public LabelDbModel? Label { get; set; }
    public SchemeDbModel? Scheme { get; set; }
}
