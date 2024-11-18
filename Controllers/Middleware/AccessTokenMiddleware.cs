using Serilog;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using JustLabel.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace JustLabel.Middleware;

public class AccessTokenMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;


    public AccessTokenMiddleware(RequestDelegate next)
    {
        _next = next;
        _logger = Log.ForContext<AccessTokenMiddleware>();
    }

    public async Task Invoke(HttpContext context)
    {
        _logger.Debug("Attempt to verify access-token");
        _logger.Debug(context.Request.Path);

        if (context.Request.Path.ToString().Contains("/api") && context.Request.Path.ToString().Contains("/auth") && context.Request.Method != "GET")
        {
            _logger.Debug("Access-token not verified: Auth");
            await _next(context);
            return;
        }

        string accessToken = context.Request.Headers["Authorization"];
        int parsedId = JWTGenerator.ValidateAccessToken(accessToken);

        if (parsedId < 0)
        {
            context.Response.StatusCode = 401;
            _logger.Debug("Access-token not verified: Wrong token");
            return;
        }

        context.Items["UserId"] = parsedId;

        _logger.Debug("Access-token verified");

        await _next(context);
    }
}
