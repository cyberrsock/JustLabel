using System;
using JustLabel.Models;
using JustLabel.Data.Models;

namespace UnitTests.Factories;

public static class ReportDbModelFactory
{
    public static ReportDbModel Create(int id, int markedId, int creatorId, string comment, DateTime loadDatetime)
    {
        return new ReportDbModel
        {
            Id = id,
            MarkedId = markedId,
            CreatorId = creatorId,
            Comment = comment,
            LoadDatetime = loadDatetime
        };
    }

    public static ReportDbModel Create(ReportModel model)
    {
        return new ReportDbModel
        {
            Id = model.Id,
            MarkedId = model.MarkedId,
            CreatorId = model.CreatorId,
            Comment = model.Comment,
            LoadDatetime = model.LoadDatetime
        };
    }
}
