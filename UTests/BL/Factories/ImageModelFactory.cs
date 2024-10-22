using JustLabel.Models;
using JustLabel.Data.Models;

namespace UnitTests.Factories;

public static class ImageModelFactory
{
    public static ImageModel Create(int id, int datasetId, string path, int width, int height)
    {
        return new ImageModel
        {
            Id = id,
            DatasetId = datasetId,
            Path = path,
            Width = width,
            Height = height
        };
    }

    public static ImageModel Create(ImageDbModel model)
    {
        return new ImageModel
        {
            Id = model.Id,
            DatasetId = model.DatasetId,
            Path = model.Path,
            Width = model.Width,
            Height = model.Height
        };
    }
}
