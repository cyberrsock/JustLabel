using Microsoft.AspNetCore.Http;
using System;
using JustLabel.Models;
using System.Collections.Generic;

namespace JustLabel.DTOModels;

public class SchemeDTOModel
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int CreatorId { get; set; }

    public List<LabelDTOModel> LabelIds { get; set; }

    public DateTime CreateDatetime { get; set; }
}
