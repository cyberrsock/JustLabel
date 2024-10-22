using System;
using System.Collections.Generic;
using JustLabel.Data.Models;

namespace IntegrationTests.Builders;

public class MarkedDbModelBuilder
{
    private MarkedDbModel _markedDbo = new();

    public MarkedDbModelBuilder WithId(int id)
    {
        _markedDbo.Id = id;
        return this;
    }

    public MarkedDbModelBuilder WithSchemeId(int schemeId)
    {
        _markedDbo.SchemeId = schemeId;
        return this;
    }

    public MarkedDbModelBuilder WithImageId(int imageId)
    {
        _markedDbo.ImageId = imageId;
        return this;
    }

    public MarkedDbModelBuilder WithCreatorId(int creatorId)
    {
        _markedDbo.CreatorId = creatorId;
        return this;
    }

    public MarkedDbModelBuilder WithIsBlocked(bool isBlocked)
    {
        _markedDbo.IsBlocked = isBlocked;
        return this;
    }

    public MarkedDbModelBuilder WithCreateDatetime(DateTime createDatetime)
    {
        _markedDbo.CreateDatetime = createDatetime;
        return this;
    }

    public MarkedDbModel Build()
    {
        return _markedDbo;
    }
}
