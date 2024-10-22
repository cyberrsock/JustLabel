using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using NpgsqlTypes;

namespace JustLabel.Data.Models;

[Table("Areas")]
public class AreaDbModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey("Label")]
    [Column("labelID")]
    public int LabelId { get; set; }

    [Column("coords")] 
    public NpgsqlPoint[] Coords { get; set; }

    // [Column("coords")] 
    // public string CoordinatesJson 
    // { 
    //     get => JsonConvert.SerializeObject(Coords);
    //     set => Coords = JsonConvert.DeserializeObject<DbPoint[]>(value);
    // }

    public LabelDbModel? Label { get; set; }

    public ICollection<MarkedDbModel> Marks {get;set;} = [];
    public ICollection<MarkedAreaDbModel> MarksAreas {get;set;} = [];
}

public class DbPoint
{
    public double X { get; set; }
    public double Y { get; set; }

    public static implicit operator DbPoint(NpgsqlPoint point)
    {
        return new DbPoint { X = point.X, Y = point.Y };
    }

    public static implicit operator NpgsqlPoint(DbPoint point)
    {
        return new NpgsqlPoint(point.X, point.Y);
    }
}
