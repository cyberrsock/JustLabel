using System;
using JustLabel.Models;
using JustLabel.Data.Models;

namespace IntegrationTests.Factories;

public static class SchemeDbModelFactory
{
    public static SchemeDbModel Create(
        int id,
        string title,
        string description,
        int creatorId,
        DateTime createDatetime)
    {
        return new SchemeDbModel
        {
            Id = id,
            Title = title,
            Description = description,
            CreatorId = creatorId,
            CreateDatetime = createDatetime
        };
    }

    public static SchemeDbModel Create(SchemeModel model)
    {
        return new SchemeDbModel
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            CreatorId = model.CreatorId,
            CreateDatetime = model.CreateDatetime
        };
    }
}
