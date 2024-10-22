using JustLabel.Models;
using JustLabel.Data.Models;

namespace JustLabel.DataMongoDb.Converters;

public static class AggregatedConverter
{
    public static AggregatedDbModel? CoreToDbModel(AggregatedModel? model)
    {
        return model is null ? null : new () {
            ImageId = model.ImageId,
            LabelId = model.LabelId,
            X1 = model.X1,
            Y1 = model.Y1,
            X2 = model.X2,
            Y2 = model.Y2
        };
    }

    public static AggregatedModel? DbToCoreModel(AggregatedDbModel? model)
    {
        return model is null ? null : new () {
            ImageId = model.ImageId,
            LabelId = model.LabelId,
            X1 = model.X1,
            Y1 = model.Y1,
            X2 = model.X2,
            Y2 = model.Y2
        };
    }
}
