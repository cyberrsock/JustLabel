using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using JustLabel.Models;

namespace JustLabel.DTOModels;

public class BanDTOModel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int AdminId { get; set; }

    public string Reason { get; set; }

    public DateTime BanDatetime { get; set; }
}
