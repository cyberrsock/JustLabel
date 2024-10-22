using System.Linq;
using System.Collections.Generic;
using JustLabel.Models;
using JustLabel.Data.Models;

namespace JustLabel.Data.Converters;

public static class MarkedConverter
{
    public static MarkedDbModel? CoreToDbModel(MarkedModel? model)
    {
        if (model is null) return null;

        var schemeDbModel = new MarkedDbModel
        {
            Id = model.Id,
            SchemeId = model.SchemeId,
            ImageId = model.ImageId,
            CreatorId = model.CreatorId,
            IsBlocked = model.IsBlocked,
            CreateDatetime = model.CreateDatetime
        };

        return schemeDbModel;
    }

    public static List<MarkedAreaDbModel> CoreToDbConnectModel(MarkedModel? model)
    {
        if (model is null) return null;

        var labelSchemeDbModels = model.AreaModels?.Select(AreaId => new MarkedAreaDbModel
        {
            AreaId = AreaId.Id,
            MarkedId = model.Id
        }).ToList() ?? new List<MarkedAreaDbModel>();

        return labelSchemeDbModels;
    }

    public static MarkedModel? DbToCoreModel(MarkedDbModel? model, List<MarkedAreaDbModel> markAreaModels, List<AreaDbModel> areaDbModels)
    {
        if (model is null) return null;

        var areaModels = markAreaModels
            .Where(markArea => markArea.MarkedId == model.Id)
            .Select(markArea => AreaConverter.DbToCoreModel(areaDbModels.FirstOrDefault(area => area.Id == markArea.AreaId)))
            .ToList();

        return new MarkedModel
        {
            Id = model.Id,
            SchemeId = model.SchemeId,
            ImageId = model.ImageId,
            CreatorId = model.CreatorId,
            IsBlocked = model.IsBlocked,
            CreateDatetime = model.CreateDatetime,
            AreaModels = areaModels
        };
    }
}
