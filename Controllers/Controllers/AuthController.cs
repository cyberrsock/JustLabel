using System;
using Microsoft.AspNetCore.Mvc;
using JustLabel.Models;
using JustLabel.Services.Interfaces;
using JustLabel.Exceptions;
using JustLabel.DTOModels;
using Microsoft.Extensions.Caching.Memory;
using JustLabel.Services;

namespace JustLabel.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private IAuthService _authService;
    private IUserService _userService;
    private IMemoryCache _cache;

    public AuthController(IAuthService authService, IUserService userService, IMemoryCache cache)
    {
        _authService = authService;
        _userService = userService;
        _cache = cache;
    }

    [HttpPost]
    public object Register(AuthDTOModel model)
    {
        try
        {
            UserModel userModel = new UserModel
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password
            };

            return Ok(_authService.Register(userModel));
        }
        catch (FailedRegistrationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (UserExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpPatch]
    public object Login(AuthDTOModel model)
    {
        try
        {
            UserModel userModel = new UserModel
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password
            };

            var tokens = _authService.Login(userModel);
            var code = _userService.SendMailPassword(userModel.Id);
            _cache.Set($"verification_code_{code}", tokens, TimeSpan.FromMinutes(5));
            return Ok();
        }
        catch (UserNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (BannedUserException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (FailedLoginException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public object CheckToken()
    {
        return Ok("Token is valid");
    }

    [HttpPatch("{code}")]
    public object Verification(int code)
    {
        if (_cache.TryGetValue($"verification_code_{code}", out AuthModel tokens))
        {
            _cache.Remove($"verification_code_{code}");
            return Ok(tokens);
        }
        return BadRequest("Wrong Code");
    }

    [HttpPut]
    public object UpdateToken(TokenDTOModel model)
    {
        try
        {
            AuthModel authModel = new AuthModel
            {
                AccessToken = model.AccessToken,
                RefreshToken = model.RefreshToken
            };

            return Ok(_authService.UpdateToken(authModel));
        }
        catch (FailedLoginException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
