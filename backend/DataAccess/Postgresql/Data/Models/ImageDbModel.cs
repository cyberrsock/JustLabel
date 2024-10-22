using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustLabel.Data.Models;

[Table("Images")]
public class ImageDbModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey("Dataset")]
    [Column("datasetID")]
    public int DatasetId { get; set; }

    [Column("path")]
    public string Path { get; set; }

    [Column("width")]
    public int Width { get; set; }

    [Column("height")]
    public int Height { get; set; }

    public DatasetDbModel? Dataset { get; set; }

    public ICollection<MarkedDbModel> Marks {get;set;} = [];
}
