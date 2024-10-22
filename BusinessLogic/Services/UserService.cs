using Serilog;
using System.Collections.Generic;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services.Interfaces;
using JustLabel.Exceptions;

namespace JustLabel.Services;

public class UserService : IUserService
{
    private IUserRepository _userRepository;
    private readonly ILogger _logger;
    
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _logger = Log.ForContext<UserService>();
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
        if (model is not null) {
            return model.Id;
        }
        return 0;
    }
}
