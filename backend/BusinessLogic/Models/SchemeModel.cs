using System;
using System.Collections.Generic;

namespace JustLabel.Models;

public class SchemeModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int CreatorId { get; set; }

    public List<LabelModel> LabelIds { get; set; }

    public DateTime CreateDatetime { get; set; }
}
