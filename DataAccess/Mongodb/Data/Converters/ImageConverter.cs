using JustLabel.Models;
using JustLabel.DataMongoDb.Models;

namespace JustLabel.DataMongoDb.Converters;

public static class ImageConverter
{
    public static ImageDbModel? CoreToDbModel(ImageModel? model)
    {
        return model is null ? null : new()
        {
            Id = model.Id,
            DatasetId = model.DatasetId,
            Path = model.Path,
            Width = model.Width,
            Height = model.Height
        };
    }

    public static ImageModel? DbToCoreModel(ImageDbModel? model)
    {
        return model is null ? null : new()
        {
            Id = model.Id,
            DatasetId = model.DatasetId,
            Path = model.Path,
            Width = model.Width,
            Height = model.Height
        };
    }
}
