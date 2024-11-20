using JustLabel.Models;
using JustLabel.Data.Models;
using System.Linq;
using NpgsqlTypes;

namespace JustLabel.Data.Converters;

public static class AreaConverter
{
    public static AreaDbModel? CoreToDbModel(AreaModel? model)
    {
        return model is null ? null : new()
        {
            Id = model.Id,
            LabelId = model.LabelId,
            Coords = model.Coords.Select(coord => new NpgsqlPoint { X = coord.X, Y = coord.Y }).ToArray()
        };
    }

    public static AreaModel? DbToCoreModel(AreaDbModel? model)
    {
        return model is null ? null : new()
        {
            Id = model.Id,
            LabelId = model.LabelId,
            Coords = model.Coords.Select(coord => new Point { X = coord.X, Y = coord.Y }).ToArray()
        };
    }
}
