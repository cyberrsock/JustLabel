using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace JustLabel.DataMongoDb.Models;

public class MarkedDbModel
{
    [Key]
    [Column("id")]
    [BsonId]
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

    [Column("loadDatetime")]
    public DateTime CreateDatetime { get; set; }
}
