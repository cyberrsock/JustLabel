using JustLabel.Models;
using JustLabel.Data.Models;
using System;
using System.Collections.Generic;

namespace IntegrationTests.Factories;

public static class MarkedModelFactory
{
    public static MarkedModel Create(
        int id,
        int schemeId,
        int imageId,
        int creatorId,
        bool isBlocked,
        DateTime createDatetime,
        List<AreaModel> areaModels = null)
    {
        return new MarkedModel
        {
            Id = id,
            SchemeId = schemeId,
            ImageId = imageId,
            CreatorId = creatorId,
            IsBlocked = isBlocked,
            CreateDatetime = createDatetime,
            AreaModels = areaModels ?? new List<AreaModel>()
        };
    }

    public static MarkedModel Create(MarkedDbModel model)
    {
        return new MarkedModel
        {
            Id = model.Id,
            SchemeId = model.SchemeId,
            ImageId = model.ImageId,
            CreatorId = model.CreatorId,
            IsBlocked = model.IsBlocked,
            CreateDatetime = model.CreateDatetime
        };
    }
}
