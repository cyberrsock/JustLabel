using System;
using System.Linq;
using JustLabel.Models;
using JustLabel.Data.Models;
using NpgsqlTypes;

namespace UnitTests.Factories;

public static class AreaDbModelFactory
{
    public static AreaDbModel Create(int id, int labelId, (double X, double Y)[] coords)
    {
        return new AreaDbModel
        {
            Id = id,
            LabelId = labelId,
            Coords = coords.Select(c => new NpgsqlPoint(c.X, c.Y)).ToArray()
        };
    }

    public static AreaDbModel Create(AreaModel model)
    {
        return new AreaDbModel
        {
            Id = model.Id,
            LabelId = model.LabelId,
            Coords = model.Coords.Select(c => new NpgsqlPoint(c.X, c.Y)).ToArray()
        };
    }
}
