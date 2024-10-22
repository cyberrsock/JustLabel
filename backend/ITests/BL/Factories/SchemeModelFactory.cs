using JustLabel.Models;
using JustLabel.Data.Models;
using System;
using System.Collections.Generic;

namespace IntegrationTests.Factories;

public static class SchemeModelFactory
{
    public static SchemeModel Create(
        int id,
        string title,
        string description,
        int creatorId,
        List<LabelModel> labelIds,
        DateTime createDatetime)
    {
        return new SchemeModel
        {
            Id = id,
            Title = title,
            Description = description,
            CreatorId = creatorId,
            LabelIds = labelIds,
            CreateDatetime = createDatetime
        };
    }

    public static SchemeModel Create(SchemeDbModel model)
    {
        return new SchemeModel
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            CreatorId = model.CreatorId,
            CreateDatetime = model.CreateDatetime
        };
    }
}
