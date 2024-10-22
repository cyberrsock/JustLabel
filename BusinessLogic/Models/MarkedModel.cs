using System;
using System.Collections.Generic;

namespace JustLabel.Models;

public class MarkedModel
{
    public int Id { get; set; }

    public int SchemeId { get; set; }

    public int ImageId { get; set; }

    public int CreatorId { get; set; }

    public bool IsBlocked { get; set; }

    public DateTime CreateDatetime { get; set; }

    public List<AreaModel> AreaModels { get; set; }
}
