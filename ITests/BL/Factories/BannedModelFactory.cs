using System;
using JustLabel.Models;
using JustLabel.Data.Models;

namespace IntegrationTests.Factories;

public static class BannedModelFactory
{
    public static BannedModel Create(int id, int userId, int adminId, string reason, DateTime banDatetime)
    {
        return new BannedModel
        {
            Id = id,
            UserId = userId,
            AdminId = adminId,
            Reason = reason,
            BanDatetime = banDatetime
        };
    }

    public static BannedModel Create(BannedDbModel model)
    {
        return new BannedModel
        {
            Id = model.Id,
            UserId = model.UserId,
            AdminId = model.AdminId,
            Reason = model.Reason,
            BanDatetime = model.BanDatetime
        };
    }
}
