using System;

namespace JustLabel.Models;

public class ReportModel
{
    public int Id { get; set; }

    public int MarkedId { get; set; }

    public int CreatorId { get; set; }

    public string Comment { get; set; }

    public DateTime LoadDatetime { get; set; }
}
