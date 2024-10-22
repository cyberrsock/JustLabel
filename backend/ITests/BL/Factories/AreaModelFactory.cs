using System;
using JustLabel.Models;
using JustLabel.Data.Models;

namespace IntegrationTests.Factories;

public static class AreaModelFactory
{
    public static AreaModel Create(int id, int labelId, (double X, double Y)[] coords)
    {
        return new AreaModel
        {
            Id = id,
            LabelId = labelId,
            Coords = Array.ConvertAll(coords, coord => new Point { X = coord.X, Y = coord.Y })
        };
    }

    public static AreaModel Create(AreaDbModel model)
    {
        return new AreaModel
        {
            Id = model.Id,
            LabelId = model.LabelId,
            Coords = Array.ConvertAll(model.Coords, c => new Point { X = c.X, Y = c.Y })
        };
    }
}
