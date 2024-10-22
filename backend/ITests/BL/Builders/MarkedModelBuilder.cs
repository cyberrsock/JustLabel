using JustLabel.Models;
using System;
using System.Collections.Generic;

namespace IntegrationTests.Builders;

public class MarkedModelBuilder
{
    private MarkedModel _markedModel = new();

    public MarkedModelBuilder WithId(int id)
    {
        _markedModel.Id = id;
        return this;
    }

    public MarkedModelBuilder WithSchemeId(int schemeId)
    {
        _markedModel.SchemeId = schemeId;
        return this;
    }

    public MarkedModelBuilder WithImageId(int imageId)
    {
        _markedModel.ImageId = imageId;
        return this;
    }

    public MarkedModelBuilder WithCreatorId(int creatorId)
    {
        _markedModel.CreatorId = creatorId;
        return this;
    }

    public MarkedModelBuilder WithIsBlocked(bool isBlocked)
    {
        _markedModel.IsBlocked = isBlocked;
        return this;
    }

    public MarkedModelBuilder WithCreateDatetime(DateTime createDatetime)
    {
        _markedModel.CreateDatetime = createDatetime;
        return this;
    }

    public MarkedModelBuilder WithAreaModels(List<AreaModel> areaModels)
    {
        _markedModel.AreaModels = areaModels;
        return this;
    }

    public MarkedModel Build()
    {
        return _markedModel;
    }
}
