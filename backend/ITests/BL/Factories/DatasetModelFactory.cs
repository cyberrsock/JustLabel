using System;
using JustLabel.Models;
using JustLabel.Data.Models;

namespace IntegrationTests.Factories;

public static class DatasetModelFactory
{
    public static DatasetModel Create(
        int id,
        string title,
        string description,
        int imageCount,
        int creatorId,
        DateTime loadDatetime)
    {
        return new DatasetModel
        {
            Id = id,
            Title = title,
            Description = description,
            ImageCount = imageCount,
            CreatorId = creatorId,
            LoadDatetime = loadDatetime
        };
    }

    public static DatasetModel Create(DatasetDbModel model)
    {
        return new DatasetModel
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            CreatorId = model.CreatorId,
            LoadDatetime = model.LoadDatetime
        };
    }
}
