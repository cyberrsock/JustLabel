using System;
using JustLabel.Models;

namespace IntegrationTests.Builders;

public class AreaModelBuilder
{
    private AreaModel _areaModel = new();

    public AreaModelBuilder WithId(int id)
    {
        _areaModel.Id = id;
        return this;
    }

    public AreaModelBuilder WithLabelId(int labelId)
    {
        _areaModel.LabelId = labelId;
        return this;
    }

    public AreaModelBuilder WithCoords((double X, double Y)[] coords)
    {
        _areaModel.Coords = Array.ConvertAll(coords, coord => new Point { X = coord.X, Y = coord.Y });
        return this;
    }

    public AreaModel Build()
    {
        return _areaModel;
    }
}
