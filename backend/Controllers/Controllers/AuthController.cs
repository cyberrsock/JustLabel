using System;
using Microsoft.AspNetCore.Mvc;
using JustLabel.Models;
using JustLabel.Services.Interfaces;
using JustLabel.Exceptions;
using JustLabel.DTOModels;

namespace JustLabel.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
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

            return Ok(_authService.Login(userModel));
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
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet]
    public object CheckToken()
    {
        return Ok("Token is valid");
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
