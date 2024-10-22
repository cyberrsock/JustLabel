using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace JustLabel.DTOModels;

public class ReportDTOModel
{
    public int Id { get; set; }

    public int MarkedId { get; set; }

    public int CreatorId { get; set; }

    public string Comment { get; set; }

    public DateTime LoadDatetime { get; set; }
}
