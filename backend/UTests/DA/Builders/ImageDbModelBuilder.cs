using System.Collections.Generic;
using JustLabel.Data.Models;

namespace UnitTests.Builders;

public class ImageDbModelBuilder
{
    private ImageDbModel _imageDbo = new();

    public ImageDbModelBuilder WithId(int id)
    {
        _imageDbo.Id = id;
        return this;
    }

    public ImageDbModelBuilder WithDatasetId(int datasetId)
    {
        _imageDbo.DatasetId = datasetId;
        return this;
    }

    public ImageDbModelBuilder WithPath(string path)
    {
        _imageDbo.Path = path;
        return this;
    }

    public ImageDbModelBuilder WithWidth(int width)
    {
        _imageDbo.Width = width;
        return this;
    }

    public ImageDbModelBuilder WithHeight(int height)
    {
        _imageDbo.Height = height;
        return this;
    }

    public ImageDbModel Build()
    {
        return _imageDbo;
    }
}
