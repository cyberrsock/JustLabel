using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustLabel.Data.Models;

[Table("Datasets")]
public class DatasetDbModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; } 

    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [ForeignKey("User")]
    [Column("creatorID")]
    public int CreatorId { get; set; }

    [Column("loadDatetime", TypeName = "timestamp")]
    public DateTime LoadDatetime { get; set; }

    public UserDbModel? User { get; set; }

    public ICollection<ImageDbModel> Images {get;set;} = [];
}
