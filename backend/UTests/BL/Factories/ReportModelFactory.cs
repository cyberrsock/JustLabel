using JustLabel.Models;
using JustLabel.Data.Models;
using System;

namespace UnitTests.Factories;

public static class ReportModelFactory
{
    public static ReportModel Create(int id, int markedId, int creatorId, string comment, DateTime loadDatetime)
    {
        return new ReportModel
        {
            Id = id,
            MarkedId = markedId,
            CreatorId = creatorId,
            Comment = comment,
            LoadDatetime = loadDatetime
        };
    }

    public static ReportModel Create(ReportDbModel model)
    {
        return new ReportModel
        {
            Id = model.Id,
            MarkedId = model.MarkedId,
            CreatorId = model.CreatorId,
            Comment = model.Comment,
            LoadDatetime = model.LoadDatetime
        };
    }
}
