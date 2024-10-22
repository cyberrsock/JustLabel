using System.Linq;
using System.Collections.Generic;
using JustLabel.Models;
using JustLabel.DataMongoDb.Models;

namespace JustLabel.DataMongoDb.Converters;

public static class SchemeConverter
{
    public static SchemeDbModel? CoreToDbModel(SchemeModel? model)
    {
        if (model is null) return null;

        var schemeDbModel = new SchemeDbModel
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            CreatorId = model.CreatorId,
            CreateDatetime = model.CreateDatetime
        };

        return schemeDbModel;
    }

    public static List<LabelSchemeDbModel> CoreToDbConnectModel(SchemeModel? model)
    {
        if (model is null) return null;

        var labelSchemeDbModels = model.LabelIds?.Select(labelId => new LabelSchemeDbModel
        {
            LabelId = labelId.Id,
            SchemeId = model.Id
        }).ToList() ?? new List<LabelSchemeDbModel>();

        return labelSchemeDbModels;
    }

    public static SchemeModel? DbToCoreModel(SchemeDbModel? model, List<LabelSchemeDbModel> labelSchemeDbModels)
    {
        if (model is null) return null;

        var labelIds = labelSchemeDbModels
            .Where(labelScheme => labelScheme.SchemeId == model.Id)
            .Select(labelScheme => new LabelModel
            {
                Id = labelScheme.LabelId
            })
            .ToList();

        return new SchemeModel
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            CreatorId = model.CreatorId,
            LabelIds = labelIds,
            CreateDatetime = model.CreateDatetime
        };
    }
}
