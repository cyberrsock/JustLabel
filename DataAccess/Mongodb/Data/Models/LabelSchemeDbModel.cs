using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace JustLabel.DataMongoDb.Models;

public class LabelSchemeDbModel
{
    [Key]
    [Column("id")]
    [BsonId]
    public int Id { get; set; }

    [ForeignKey("Label")]
    [Column("labelID")]
    public int LabelId { get; set; }

    [ForeignKey("Scheme")]
    [Column("schemeID")]
    public int SchemeId { get; set; }
}
