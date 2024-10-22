using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace JustLabel.DTOModels;

public class AuthDTOModel
{
    public string Username { get; set; }

    public string Email { get; set; }
    
    public string Password { get; set; }
}
