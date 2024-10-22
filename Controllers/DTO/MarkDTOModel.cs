using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using JustLabel.Models;

namespace JustLabel.DTOModels;

public class MarkGroupDTOModel
{
    public int UserId { get; set; }

    public List<MarkedModel> Marks { get; set; }
}

public class RectMarkDTOModel
{
    public int Id { get; set; }

    public int SchemeId { get; set; }

    public int ImageId { get; set; }

    public int CreatorId { get; set; }

    public bool IsBlocked { get; set; }

    public DateTime CreateDatetime { get; set; }

    public List<RectAreaDTOModel> AreaModels { get; set; }
}

public class MarkDTOModel
{
    public int Id { get; set; }

    public int SchemeId { get; set; }

    public int ImageId { get; set; }

    public int CreatorId { get; set; }

    public bool IsBlocked { get; set; }

    public DateTime CreateDatetime { get; set; }

    public List<AreaDTOModel> AreaModels { get; set; }
}

