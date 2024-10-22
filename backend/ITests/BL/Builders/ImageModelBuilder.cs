using JustLabel.Models;

namespace IntegrationTests.Builders;

public class ImageModelBuilder
{
    private ImageModel _imageModel = new();

    public ImageModelBuilder WithId(int id)
    {
        _imageModel.Id = id;
        return this;
    }

    public ImageModelBuilder WithDatasetId(int datasetId)
    {
        _imageModel.DatasetId = datasetId;
        return this;
    }

    public ImageModelBuilder WithPath(string path)
    {
        _imageModel.Path = path;
        return this;
    }

    public ImageModelBuilder WithWidth(int width)
    {
        _imageModel.Width = width;
        return this;
    }

    public ImageModelBuilder WithHeight(int height)
    {
        _imageModel.Height = height;
        return this;
    }

    public ImageModel Build()
    {
        return _imageModel;
    }
}
