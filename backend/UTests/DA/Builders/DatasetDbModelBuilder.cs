using System;
using System.Collections.Generic;
using JustLabel.Data.Models;

namespace UnitTests.Builders;

public class DatasetDbModelBuilder
{
    private DatasetDbModel _datasetDbo = new();

    public DatasetDbModelBuilder WithId(int id)
    {
        _datasetDbo.Id = id;
        return this;
    }

    public DatasetDbModelBuilder WithTitle(string title)
    {
        _datasetDbo.Title = title;
        return this;
    }

    public DatasetDbModelBuilder WithDescription(string description)
    {
        _datasetDbo.Description = description;
        return this;
    }

    public DatasetDbModelBuilder WithCreatorId(int creatorId)
    {
        _datasetDbo.CreatorId = creatorId;
        return this;
    }

    public DatasetDbModelBuilder WithLoadDatetime(DateTime loadDatetime)
    {
        _datasetDbo.LoadDatetime = loadDatetime;
        return this;
    }

    public DatasetDbModel Build()
    {
        return _datasetDbo;
    }
}
