using JustLabel.Models;
using JustLabel.Data.Models;

namespace JustLabel.Data.Converters;

public static class BannedConverter
{
    public static BannedDbModel? CoreToDbModel(BannedModel? model)
    {
        return model is null ? null : new()
        {
            Id = model.Id,
            UserId = model.UserId,
            AdminId = model.AdminId,
            Reason = model.Reason,
            BanDatetime = model.BanDatetime
        };
    }

    public static BannedModel? DbToCoreModel(BannedDbModel? model)
    {
        return model is null ? null : new()
        {
            Id = model.Id,
            UserId = model.UserId,
            AdminId = model.AdminId,
            Reason = model.Reason,
            BanDatetime = model.BanDatetime
        };
    }
}
