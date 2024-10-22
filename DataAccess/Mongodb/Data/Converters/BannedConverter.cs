using JustLabel.Models;
using JustLabel.DataMongoDb.Models;

namespace JustLabel.DataMongoDb.Converters;

public static class BannedConverter
{
    public static BannedDbModel? CoreToDbModel(BannedModel? model)
    {
        return model is null ? null : new () {
            Id = model.Id,
            UserId = model.UserId,
            AdminId = model.AdminId,
            Reason = model.Reason,
            BanDatetime = model.BanDatetime
        };
    }

    public static BannedModel? DbToCoreModel(BannedDbModel? model)
    {
        return model is null ? null : new () {
            Id = model.Id,
            UserId = model.UserId,
            AdminId = model.AdminId,
            Reason = model.Reason,
            BanDatetime = model.BanDatetime
        };
    }
}
