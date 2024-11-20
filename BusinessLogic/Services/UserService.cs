using Serilog;
using System.Collections.Generic;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services.Interfaces;
using JustLabel.Exceptions;
using System.Net;
using System.Net.Mail;
using System;
using JustLabel.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace JustLabel.Services;

public class UserService : IUserService
{
    private IUserRepository _userRepository;
    private readonly ILogger _logger;
    private readonly IMemoryCache _cache;

    public UserService(IUserRepository userRepository, IMemoryCache cache)
    {
        _userRepository = userRepository;
        _logger = Log.ForContext<UserService>();
        _cache = cache;
    }

    public UserModel GetUserByID(int id)
    {
        _logger.Debug($"Attempt to get user ID{id}");

        var foundUser = _userRepository.GetUserById(id);
        if (foundUser is null)
        {
            throw new UserNotExistsException("User with this id does not exist");
        }

        _logger.Debug($"User ID{id} successfully got");

        return foundUser;
    }

    public List<UserModel> GetUserByIDs(List<int> ids)
    {
        var foundUsers = new List<UserModel>();
        foreach (var id in ids)
        {
            foundUsers.Add(_userRepository.GetUserById(id));
        }

        return foundUsers;
    }

    public void Ban(BannedModel model)
    {
        _logger.Debug($"Attempt to ban user ID{model.UserId}");

        if (_userRepository.GetUserById(model.UserId) is null)
        {
            _logger.Error($"User ID{model.UserId} does not exist");
            throw new UserNotExistsException("User with this id does not exist");
        }

        var foundBan = _userRepository.IsBan(model.UserId);
        if (foundBan is not null)
        {
            _logger.Error($"User ID{model.UserId} has already been banned");
            throw new BannedUserException("The user has already been banned. Reason: '" + foundBan.Reason + "'. Date: " + foundBan.BanDatetime);
        }

        if (!_userRepository.GetUserById(model.AdminId).IsAdmin)
        {
            _logger.Error($"User ID{model.AdminId} is not admin");
            throw new AdminUserException("The user with AdminId is not admin");
        }

        if (_userRepository.GetUserById(model.UserId).IsAdmin)
        {
            _logger.Error($"User ID{model.UserId} is admin");
            throw new AdminUserException("The user with UserId is admin");
        }

        _userRepository.Ban(model);

        _logger.Information($"Banned user ID{model.UserId}");

        _logger.Debug($"User ID{model.UserId} successfully banned");
    }

    public List<UserModel> GetUsers(int admin_id)
    {
        _logger.Debug($"Attempt to get all users");

        if (!_userRepository.GetUserById(admin_id).IsAdmin)
        {
            _logger.Error($"User ID{admin_id} is not admin");
            throw new AdminUserException("The user with AdminId is not admin");
        }

        var res = _userRepository.GetAll();
        _logger.Debug($"Success get all users");
        return res;
    }

    public void Unban(int id)
    {
        _logger.Debug($"Attempt to unban user ID{id}");

        if (_userRepository.IsBan(id) is null)
        {
            _logger.Error($"User ID{id} is not banned");
            throw new UnbannedUserException("The user is not banned");
        }

        _userRepository.Unban(id);

        _logger.Information($"Unbanned user ID{id}");

        _logger.Debug($"User ID{id} successfully unbanned");
    }

    public void UnbanByBanId(int user_id, int ban_id)
    {
        _logger.Debug($"Attempt to unban user ID{user_id}");

        if (_userRepository.IsBan(user_id) is null)
        {
            _logger.Error($"User ID{user_id} is not banned");
            throw new UnbannedUserException("The user is not banned");
        }

        _userRepository.UnbanByBanId(user_id, ban_id);

        _logger.Information($"Unbanned user ID{user_id}");

        _logger.Debug($"User ID{user_id} successfully unbanned");
    }

    public void BanMarks(int id, int admin_id, bool isBlock)
    {
        _logger.Debug($"Attempt to unban user ID{id}");

        if (!_userRepository.GetUserById(admin_id).IsAdmin)
        {
            _logger.Error($"User ID{admin_id} is not admin");
            throw new AdminUserException("The user with AdminId is not admin");
        }

        _userRepository.BanMarks(id, isBlock);
        _logger.Debug($"User ID{id} successfully unbanned");
    }

    public int IsBanned(int id)
    {
        BannedModel model = _userRepository.IsBan(id);
        if (model is not null)
        {
            return model.Id;
        }
        return 0;
    }

    public int SendMailPassword(int id)
    {
        var user = _userRepository.GetUserById(id);

        if (user is null)
        {
            _logger.Error($"User ID{id} does not exist");
            throw new UserNotExistsException("User with this id does not exist");
        }

        var email = user.Email;

        _logger.Debug($"Attempt to send password recovery email to {email}");

        string staticCodeString = Environment.GetEnvironmentVariable("STATIC_EMAIL_CODE");
        int code;

        if (!string.IsNullOrEmpty(staticCodeString))
        {
            if (int.TryParse(staticCodeString, out code))
            {
                _logger.Debug($"Using static code: {code}");
            }
            else
            {
                _logger.Error($"STATIC_EMAIL_CODE is not a valid number: {staticCodeString}");
                throw new InvalidOperationException("STATIC_EMAIL_CODE must be a valid integer.");
            }
        }
        else
        {
            code = new Random().Next(100000, 999999);
            _logger.Debug($"Generated random code: {code}");

            var mailMessage = new MailMessage(Environment.GetEnvironmentVariable("USER_EMAIL"), email)
            {
                Subject = "Your Code",
                Body = $"<p>{code}</p>",
                IsBodyHtml = true
            };

            using (var smtpClient = new SmtpClient(Environment.GetEnvironmentVariable("SMTP_HOST"))
            {
                Port = 587,
                Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("USER_EMAIL"), Environment.GetEnvironmentVariable("EMAIL_PASSWORD")),
                EnableSsl = true,
            })
            {
                try
                {
                    smtpClient.Send(mailMessage);
                    _logger.Information($"Successfully sent password recovery email to {email}");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to send password recovery email");
                    throw new Exception("An error occurred while attempting to send the password recovery email", ex);
                }
            }
        }
        return code;
    }

    public void ChangePassword(int id, int code, string password)
    {
        var user = _userRepository.GetUserById(id);
        if (user is null)
        {
            _logger.Error($"User ID{id} does not exist");
            throw new UserNotExistsException("User with this id does not exist");
        }

        if (_cache.TryGetValue($"reset_password_code_{id}", out int storedCode))
        {
            if (storedCode == code)
            {
                _cache.Remove($"reset_password_code_{id}");
                var hash_password = SaltedHash.GenerateSaltedHash(password, user.Salt);
                _userRepository.ChangePassword(id, hash_password);
                _logger.Information($"Password for user ID{id} has been successfully changed.");
            }
            else
            {
                _logger.Error($"Invalid code provided for user ID{id}");
                throw new Exception("Invalid or expired code.");
            }
        }
        else
        {
            _logger.Error($"No code found in cache for user ID{id}");
            throw new Exception("No code found or code has expired.");
        }
    }
}
