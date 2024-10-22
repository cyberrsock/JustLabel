using JustLabel.Models;
using System;
using System.Collections.Generic;

namespace UnitTests.Builders;

public class SchemeModelBuilder
{
    private SchemeModel _schemeModel = new();

    public SchemeModelBuilder WithId(int id)
    {
        _schemeModel.Id = id;
        return this;
    }

    public SchemeModelBuilder WithTitle(string title)
    {
        _schemeModel.Title = title;
        return this;
    }

    public SchemeModelBuilder WithDescription(string description)
    {
        _schemeModel.Description = description;
        return this;
    }

    public SchemeModelBuilder WithCreatorId(int creatorId)
    {
        _schemeModel.CreatorId = creatorId;
        return this;
    }

    public SchemeModelBuilder WithLabelIds(List<LabelModel> labelIds)
    {
        _schemeModel.LabelIds = labelIds;
        return this;
    }

    public SchemeModelBuilder WithCreateDatetime(DateTime createDatetime)
    {
        _schemeModel.CreateDatetime = createDatetime;
        return this;
    }

    public SchemeModel Build()
    {
        return _schemeModel;
    }
}
