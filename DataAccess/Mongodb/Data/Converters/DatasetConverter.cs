using JustLabel.Models;
using JustLabel.DataMongoDb.Models;

namespace JustLabel.DataMongoDb.Converters;

public static class DatasetConverter
{
    public static DatasetDbModel? CoreToDbModel(DatasetModel? model)
    {
        return model is null ? null : new()
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            CreatorId = model.CreatorId,
            LoadDatetime = model.LoadDatetime
        };
    }

    public static DatasetModel? DbToCoreModel(DatasetDbModel? model)
    {
        return model is null ? null : new()
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            CreatorId = model.CreatorId,
            LoadDatetime = model.LoadDatetime
        };
    }
}
