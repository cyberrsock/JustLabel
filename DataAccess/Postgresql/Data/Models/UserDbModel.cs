using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustLabel.Data.Models;

[Table("Users")]
public class UserDbModel
{
    [Key]
    [Column("id")]
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

    public ICollection<DatasetDbModel> Datasets { get; set; } = [];
    public ICollection<BannedDbModel> Banned { get; set; } = [];
    public ICollection<BannedDbModel> BannedBy { get; set; } = [];
    public ICollection<ReportDbModel> Reports { get; set; } = [];
    public ICollection<SchemeDbModel> Schemes { get; set; } = [];
    public ICollection<MarkedDbModel> Marks { get; set; } = [];

}
