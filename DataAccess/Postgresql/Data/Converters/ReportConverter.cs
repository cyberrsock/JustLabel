using JustLabel.Models;
using JustLabel.Data.Models;

namespace JustLabel.Data.Converters;

public static class ReportConverter
{
    public static ReportDbModel? CoreToDbModel(ReportModel? model)
    {
        return model is null ? null : new()
        {
            Id = model.Id,
            MarkedId = model.MarkedId,
            CreatorId = model.CreatorId,
            Comment = model.Comment,
            LoadDatetime = model.LoadDatetime
        };
    }

    public static ReportModel? DbToCoreModel(ReportDbModel? model)
    {
        return model is null ? null : new()
        {
            Id = model.Id,
            MarkedId = model.MarkedId,
            CreatorId = model.CreatorId,
            Comment = model.Comment,
            LoadDatetime = model.LoadDatetime
        };
    }
}
