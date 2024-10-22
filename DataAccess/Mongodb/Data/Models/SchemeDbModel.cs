using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace JustLabel.DataMongoDb.Models;

public class SchemeDbModel
{
    [Key]
    [Column("id")]
    [BsonId]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [ForeignKey("User")]
    [Column("creatorID")]
    public int CreatorId { get; set; }

    [Column("createDatetime")]
    public DateTime CreateDatetime { get; set; }
}
