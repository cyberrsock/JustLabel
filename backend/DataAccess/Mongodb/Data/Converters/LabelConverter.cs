using JustLabel.Models;
using JustLabel.DataMongoDb.Models;

namespace JustLabel.DataMongoDb.Converters;

public static class LabelConverter
{
    public static LabelDbModel? CoreToDbModel(LabelModel? model)
    {
        return model is null ? null : new () {
            Id = model.Id,
            Title = model.Title
        };
    }

    public static LabelModel? DbToCoreModel(LabelDbModel? model)
    {
        return model is null ? null : new () {
            Id = model.Id,
            Title = model.Title
        };
    }
}
