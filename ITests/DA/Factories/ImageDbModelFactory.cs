using JustLabel.Models;
using JustLabel.Data.Models;

namespace IntegrationTests.Factories;

public static class ImageDbModelFactory
{
    public static ImageDbModel Create(int id, int datasetId, string path, int width, int height)
    {
        return new ImageDbModel
        {
            Id = id,
            DatasetId = datasetId,
            Path = path,
            Width = width,
            Height = height
        };
    }

    public static ImageDbModel Create(ImageModel model)
    {
        return new ImageDbModel
        {
            Id = model.Id,
            DatasetId = model.DatasetId,
            Path = model.Path,
            Width = model.Width,
            Height = model.Height
        };
    }
}
