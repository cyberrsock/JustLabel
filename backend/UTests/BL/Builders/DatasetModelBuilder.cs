using System;
using JustLabel.Models;

namespace UnitTests.Builders;

public class DatasetModelBuilder
{
    private DatasetModel _datasetModel = new();

    public DatasetModelBuilder WithId(int id)
    {
        _datasetModel.Id = id;
        return this;
    }

    public DatasetModelBuilder WithTitle(string title)
    {
        _datasetModel.Title = title;
        return this;
    }

    public DatasetModelBuilder WithDescription(string description)
    {
        _datasetModel.Description = description;
        return this;
    }

    public DatasetModelBuilder WithImageCount(int imageCount)
    {
        _datasetModel.ImageCount = imageCount;
        return this;
    }

    public DatasetModelBuilder WithCreatorId(int creatorId)
    {
        _datasetModel.CreatorId = creatorId;
        return this;
    }

    public DatasetModelBuilder WithLoadDatetime(DateTime loadDatetime)
    {
        _datasetModel.LoadDatetime = loadDatetime;
        return this;
    }

    public DatasetModel Build()
    {
        return _datasetModel;
    }
}
