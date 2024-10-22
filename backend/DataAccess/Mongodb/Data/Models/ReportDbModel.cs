using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace JustLabel.DataMongoDb.Models;

public class ReportDbModel
{
    [Key]
    [Column("id")]
    [BsonId]
    public int Id { get; set; }

    [ForeignKey("Marked")]
    [Column("markedID")]
    public int MarkedId { get; set; }

    [ForeignKey("User")]
    [Column("creatorID")]
    public int CreatorId { get; set; }

    [Column("comment")]
    public string Comment { get; set; }

    [Column("loadDatetime")]
    public DateTime LoadDatetime { get; set; }
}
