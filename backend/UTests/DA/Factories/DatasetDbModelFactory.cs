using System;
using JustLabel.Models;
using JustLabel.Data.Models;

namespace UnitTests.Factories;

public static class DatasetDbModelFactory
{
    public static DatasetDbModel Create(
        int id,
        string title,
        string description,
        int creatorId,
        DateTime loadDatetime)
    {
        return new DatasetDbModel
        {
            Id = id,
            Title = title,
            Description = description,
            CreatorId = creatorId,
            LoadDatetime = loadDatetime
        };
    }

    public static DatasetDbModel Create(DatasetModel model)
    {
        return new DatasetDbModel
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            CreatorId = model.CreatorId,
            LoadDatetime = model.LoadDatetime
        };
    }
}
