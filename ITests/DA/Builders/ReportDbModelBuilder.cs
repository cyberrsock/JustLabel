using System;
using JustLabel.Data.Models;

namespace IntegrationTests.Builders;

public class ReportDbModelBuilder
{
    private ReportDbModel _reportDbo = new();

    public ReportDbModelBuilder WithId(int id)
    {
        _reportDbo.Id = id;
        return this;
    }

    public ReportDbModelBuilder WithMarkedId(int markedId)
    {
        _reportDbo.MarkedId = markedId;
        return this;
    }

    public ReportDbModelBuilder WithCreatorId(int creatorId)
    {
        _reportDbo.CreatorId = creatorId;
        return this;
    }

    public ReportDbModelBuilder WithComment(string comment)
    {
        _reportDbo.Comment = comment;
        return this;
    }

    public ReportDbModelBuilder WithLoadDatetime(DateTime loadDatetime)
    {
        _reportDbo.LoadDatetime = loadDatetime;
        return this;
    }

    public ReportDbModel Build()
    {
        return _reportDbo;
    }
}
