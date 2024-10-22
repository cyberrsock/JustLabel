using System;
using JustLabel.Data.Models;

namespace UnitTests.Builders;

public class BannedDbModelBuilder
{
    private BannedDbModel _bannedDbo = new();

    public BannedDbModelBuilder WithId(int id)
    {
        _bannedDbo.Id = id;
        return this;
    }

    public BannedDbModelBuilder WithUserId(int userId)
    {
        _bannedDbo.UserId = userId;
        return this;
    }

    public BannedDbModelBuilder WithAdminId(int adminId)
    {
        _bannedDbo.AdminId = adminId;
        return this;
    }

    public BannedDbModelBuilder WithReason(string reason)
    {
        _bannedDbo.Reason = reason;
        return this;
    }

    public BannedDbModelBuilder WithBanDatetime(DateTime banDatetime)
    {
        _bannedDbo.BanDatetime = banDatetime;
        return this;
    }

    public BannedDbModel Build()
    {
        return _bannedDbo;
    }
}
