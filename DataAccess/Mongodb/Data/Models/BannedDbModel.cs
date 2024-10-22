using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace JustLabel.DataMongoDb.Models;

public class BannedDbModel
{
    [Key]
    [Column("id")]
    [BsonId]
    public int Id { get; set; }

    [ForeignKey("User")]
    [Column("userID")]
    public int UserId { get; set; }
    
    [ForeignKey("User")]
    [Column("adminID")]
    public int AdminId { get; set; }

    [Column("reason")]
    public string Reason { get; set; }

    [Column("banDatetime")]
    public DateTime BanDatetime { get; set; }
}
