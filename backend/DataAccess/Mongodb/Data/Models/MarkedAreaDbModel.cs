using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace JustLabel.DataMongoDb.Models;

public class MarkedAreaDbModel
{
    [Key]
    [Column("id")]
    [BsonId]
    public int Id { get; set; }

    [ForeignKey("Marked")]
    [Column("markedID")]
    public int MarkedId { get; set; }

    [ForeignKey("Area")]
    [Column("areaID")]
    public int AreaId { get; set; }
}
