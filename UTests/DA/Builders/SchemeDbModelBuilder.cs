using System;
using System.Collections.Generic;
using JustLabel.Data.Models;

namespace UnitTests.Builders;

public class SchemeDbModelBuilder
{
    private SchemeDbModel _schemeDbo = new();

    public SchemeDbModelBuilder WithId(int id)
    {
        _schemeDbo.Id = id;
        return this;
    }

    public SchemeDbModelBuilder WithTitle(string title)
    {
        _schemeDbo.Title = title;
        return this;
    }

    public SchemeDbModelBuilder WithDescription(string description)
    {
        _schemeDbo.Description = description;
        return this;
    }

    public SchemeDbModelBuilder WithCreatorId(int creatorId)
    {
        _schemeDbo.CreatorId = creatorId;
        return this;
    }

    public SchemeDbModelBuilder WithCreateDatetime(DateTime createDatetime)
    {
        _schemeDbo.CreateDatetime = createDatetime;
        return this;
    }

    public SchemeDbModel Build()
    {
        return _schemeDbo;
    }
}
