using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace JustLabel.DTOModels;

public class TokenDTOModel
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}
