using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace JustLabel.DataMongoDb.Models;

public class ImageDbModel
{
    [Key]
    [Column("id")]
    [BsonId]
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
}
