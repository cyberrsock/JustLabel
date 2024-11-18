using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using JustLabel.Models;
using JustLabel.DTOModels;
using JustLabel.Services.Interfaces;
using JustLabel.Exceptions;
using System;
using Microsoft.Extensions.Caching.Memory;

namespace JustLabel.Controllers;

[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;
    private IMemoryCache _cache;

    public UsersController(IUserService userService, IMemoryCache cache)
    {
        _userService = userService;
        _cache = cache;
    }

    [HttpGet("{id}")]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public object Get(int id)
    {
        return _userService.GetUserByID(id);
    }

    [HttpGet]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public object Get([FromQuery] List<int> ids)
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        if (ids.Count > 0)
        {
            var foundUsers = new List<UserDTOModel>();
            var users = _userService.GetUserByIDs(ids);
            foreach (var user in users)
            {
                foundUsers.Add(new UserDTOModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    BanId = _userService.IsBanned(user.Id),
                    BlockMarks = user.BlockMarks
                });
            }
            return Ok(foundUsers);
        }
        else
        {
            var foundUsers = new List<UserDTOModel>();
            var users = _userService.GetUsers(userId);
            foreach (var user in users)
            {
                foundUsers.Add(new UserDTOModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    BanId = _userService.IsBanned(user.Id),
                    BlockMarks = user.BlockMarks
                });
            }
            return Ok(foundUsers);
        }

    }

    [HttpPut]
    [MapToApiVersion("1.0")]
    public object Ban(BanDTOModel model)
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        model.AdminId = userId;

        BannedModel banModel = new BannedModel
        {
            Id = model.Id,
            UserId = model.UserId,
            AdminId = model.AdminId,
            Reason = model.Reason,
            BanDatetime = model.BanDatetime
        };

        _userService.Ban(banModel);

        return Ok();
    }

    [HttpPatch("{id}")]
    [MapToApiVersion("1.0")]
    [MapToApiVersion("2.0")]
    public object BanMarks(UserDTOModel model)
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        _userService.BanMarks(model.Id, userId, model.BlockMarks);

        return Ok();
    }

    [HttpDelete("{id}")]
    [MapToApiVersion("1.0")]
    public object Unban(int id)
    {
        _userService.Unban(id);

        return Ok();
    }

    [HttpPost("{user_id}/blocks")]
    [MapToApiVersion("2.0")]
    public object Ban2(int user_id, [FromBody] BanDTOModel model)
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        model.AdminId = userId;
        model.UserId = user_id;

        BannedModel banModel = new BannedModel
        {
            Id = model.Id,
            UserId = model.UserId,
            AdminId = model.AdminId,
            Reason = model.Reason,
            BanDatetime = model.BanDatetime
        };

        _userService.Ban(banModel);

        return Ok();
    }

    [HttpDelete("{user_id}/blocks/{block_id}")]
    [MapToApiVersion("2.0")]
    public object Unban2(int user_id, int block_id)
    {
        _userService.Unban(user_id);

        return Ok();
    }

    [HttpPut]
    [MapToApiVersion("2.0")]
    public object SendMailPassword()
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        var code = _userService.SendMailPassword(userId);
        _cache.Set($"reset_password_code_{userId}", code, TimeSpan.FromMinutes(15));

        return Ok();
    }

    [HttpPut("{code}")]
    [MapToApiVersion("2.0")]
    public object ChangePassword(int code, string password = "test123")
    {
        if (!HttpContext.Items.ContainsKey("UserId") || HttpContext.Items["UserId"] is not int userId)
        {
            return BadRequest("User ID not found in the HttpContext");
        }

        try
        {
            _userService.ChangePassword(userId, code, password);
            return Ok();
        }
        catch (UserNotExistsException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
