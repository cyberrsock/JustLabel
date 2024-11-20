using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustLabel.Data.Models;

[Table("Banned")]
public class BannedDbModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey("User")]
    [Column("userID")]
    public int UserId { get; set; }

    [ForeignKey("Admin")]
    [Column("adminID")]
    public int AdminId { get; set; }

    [Column("reason")]
    public string Reason { get; set; }

    [Column("banDatetime", TypeName = "timestamp")]
    public DateTime BanDatetime { get; set; }

    public UserDbModel? User { get; set; }
    public UserDbModel? Admin { get; set; }

}
