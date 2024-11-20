using JustLabel.Models;
using JustLabel.DataMongoDb.Models;
using System.Linq;

namespace JustLabel.DataMongoDb.Converters;

public static class AreaConverter
{
    public static AreaDbModel? CoreToDbModel(AreaModel? model)
    {
        return model is null ? null : new()
        {
            Id = model.Id,
            LabelId = model.LabelId,
            Coords = model.Coords.Select(coord => new DbPoint { X = coord.X, Y = coord.Y }).ToArray()
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
