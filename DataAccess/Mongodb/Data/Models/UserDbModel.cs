using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace JustLabel.DataMongoDb.Models;

public class UserDbModel
{
    [Key]
    [Column("id")]
    [BsonId]
    public int Id { get; set; }

    [Column("username")]
    public string Username { get; set; }

    [Column("email")]
    public string Email { get; set; }

    [Column("password")]
    public string Password { get; set; }

    [Column("salt")]
    public string Salt { get; set; }

    [Column("refreshToken")]
    public string RefreshToken { get; set; }

    [Column("isAdmin")]
    public bool IsAdmin { get; set; }

    [Column("blockMarks")]
    public bool BlockMarks { get; set; }
}
