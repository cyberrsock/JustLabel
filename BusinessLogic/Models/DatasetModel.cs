using System;

namespace JustLabel.Models;

public class DatasetModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int ImageCount { get; set; }

    public int CreatorId { get; set; }

    public DateTime LoadDatetime { get; set; }
}
