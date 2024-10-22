using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace JustLabel.DTOModels;

public class UserDTOModel
{
    public int Id { get; set; }

    public string Username { get; set; }

    public int BanId { get; set; }

    public bool BlockMarks { get; set; }
}
