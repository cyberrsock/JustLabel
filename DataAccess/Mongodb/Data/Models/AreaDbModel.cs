using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace JustLabel.DataMongoDb.Models;

public class AreaDbModel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("labelID")]
    public int LabelId { get; set; }

    [Column("coords")]
    public DbPoint[] Coords { get; set; }
}

public class DbPoint
{
    public double X { get; set; }
    public double Y { get; set; }
}