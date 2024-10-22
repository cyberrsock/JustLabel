using System;
using JustLabel.Models;
using JustLabel.Data.Models;

namespace IntegrationTests.Factories;

public static class BannedDbModelFactory
{
    public static BannedDbModel Create(int id, int userId, int adminId, string reason, DateTime banDatetime)
    {
        return new BannedDbModel
        {
            Id = id,
            UserId = userId,
            AdminId = adminId,
            Reason = reason,
            BanDatetime = banDatetime
        };
    }

    public static BannedDbModel Create(BannedModel model)
    {
        return new BannedDbModel
        {
            Id = model.Id,
            UserId = model.UserId,
            AdminId = model.AdminId,
            Reason = model.Reason,
            BanDatetime = model.BanDatetime
        };
    }
}
