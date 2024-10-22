using JustLabel.Models;
using System;

namespace IntegrationTests.Builders;

public class ReportModelBuilder
{
    private ReportModel _reportModel = new();

    public ReportModelBuilder WithId(int id)
    {
        _reportModel.Id = id;
        return this;
    }

    public ReportModelBuilder WithMarkedId(int markedId)
    {
        _reportModel.MarkedId = markedId;
        return this;
    }

    public ReportModelBuilder WithCreatorId(int creatorId)
    {
        _reportModel.CreatorId = creatorId;
        return this;
    }

    public ReportModelBuilder WithComment(string comment)
    {
        _reportModel.Comment = comment;
        return this;
    }

    public ReportModelBuilder WithLoadDatetime(DateTime loadDatetime)
    {
        _reportModel.LoadDatetime = loadDatetime;
        return this;
    }

    public ReportModel Build()
    {
        return _reportModel;
    }
}
