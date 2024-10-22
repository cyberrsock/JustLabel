using System;
using JustLabel.Models;

namespace IntegrationTests.Builders;

public class BannedModelBuilder
{
    private BannedModel _bannedModel = new();

    public BannedModelBuilder WithId(int id)
    {
        _bannedModel.Id = id;
        return this;
    }

    public BannedModelBuilder WithUserId(int userId)
    {
        _bannedModel.UserId = userId;
        return this;
    }

    public BannedModelBuilder WithAdminId(int adminId)
    {
        _bannedModel.AdminId = adminId;
        return this;
    }

    public BannedModelBuilder WithReason(string reason)
    {
        _bannedModel.Reason = reason;
        return this;
    }

    public BannedModelBuilder WithBanDatetime(DateTime banDatetime)
    {
        _bannedModel.BanDatetime = banDatetime;
        return this;
    }

    public BannedModel Build()
    {
        return _bannedModel;
    }
}
