using System;

namespace JustLabel.Models;

public class BannedModel
{
    public int Id { get; set; }

    public int UserId { get; set; }
    
    public int AdminId { get; set; }

    public string Reason { get; set; }

    public DateTime BanDatetime { get; set; }
}
