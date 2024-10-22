using System;
using JustLabel.Models;
using JustLabel.Data.Models;

namespace IntegrationTests.Factories;

public static class MarkedDbModelFactory
{
    public static MarkedDbModel Create(int id, int schemeId, int imageId, int creatorId, bool isBlocked, DateTime createDatetime)
    {
        return new MarkedDbModel
        {
            Id = id,
            SchemeId = schemeId,
            ImageId = imageId,
            CreatorId = creatorId,
            IsBlocked = isBlocked,
            CreateDatetime = createDatetime
        };
    }

    public static MarkedDbModel Create(MarkedModel model)
    {
        return new MarkedDbModel
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
