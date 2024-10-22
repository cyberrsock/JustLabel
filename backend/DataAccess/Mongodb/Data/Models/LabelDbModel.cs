using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace JustLabel.DataMongoDb.Models;

public class LabelDbModel
{
    [Key]
    [Column("id")]
    [BsonId]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; }
}
