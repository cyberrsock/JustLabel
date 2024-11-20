using System;
using System.Linq;
using System.Collections.Generic;
using Serilog;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.Data;
using JustLabel.Data.Converters;

namespace JustLabel.Repositories;

public class UserRepository : IUserRepository
{
    private AppDbContext _context;
    private readonly ILogger _logger;

    public UserRepository(AppDbContext context)
    {
        _context = context;
        _logger = Log.ForContext<UserRepository>();
    }

    public void Add(UserModel model)
    {
        _logger.Debug($"Attempt to add a user {model.Username}");
        _context.Users.Add(UserConverter.CoreToDbModel(model));
        _context.SaveChanges();
        _logger.Debug($"User {model.Username} successfully added");
    }

    public void Ban(BannedModel model)
    {
        _logger.Debug($"Attempt to ban a user {model.UserId}");
        model.BanDatetime = DateTime.Now;
        _context.Banned.Add(BannedConverter.CoreToDbModel(model));
        _context.SaveChanges();
        _logger.Debug($"User {model.UserId} successfully banned");
    }

    public UserModel? GetUserByEmail(string email)
    {
        _logger.Debug($"Attempt to get a user by email {email}");
        UserModel res = UserConverter.DbToCoreModel(_context.Users.FirstOrDefault(u => u.Email == email));
        _logger.Debug($"User with email {email} successfully got");
        return res;
    }

    public UserModel? GetUserById(int id)
    {
        _logger.Debug($"Attempt to get a user ID{id}");
        UserModel res = UserConverter.DbToCoreModel(_context.Users.FirstOrDefault(u => u.Id == id));
        _logger.Debug($"User ID{id} successfully got");
        return res;
    }

    public UserModel? GetUserByUsername(string username)
    {
        try
        {
            _logger.Debug($"Attempt to get a user by username {username}");
            var userDbModel = _context.Users.FirstOrDefault(u => u.Username == username);

            if (userDbModel == null)
            {
                _logger.Debug($"User with username {username} not found");
                return null;
            }

            UserModel res = UserConverter.DbToCoreModel(userDbModel);
            _logger.Debug($"User with username {username} successfully got");

            return res;
        }
        catch (Exception ex)
        {
            _logger.Error($"An error occurred while retrieving user by username {username}: {ex.Message}");
            // Дополнительно обработайте исключение здесь, если необходимо
            return null; // Верните значение по умолчанию или null в случае исключения
        }
    }

    public List<UserModel> GetAll()
    {
        _logger.Debug($"Attempt to get users");
        var userDbModels = _context.Users.ToList().Where(u => u.IsAdmin == false);
        List<UserModel> res = userDbModels.Select(model => UserConverter.DbToCoreModel(model)).ToList();
        _logger.Debug($"Users successfully got");
        return res;
    }

    public BannedModel? IsBan(int id)
    {
        _logger.Debug($"Attempt to get a user ID{id}");
        BannedModel res = BannedConverter.DbToCoreModel(_context.Banned.FirstOrDefault(b => b.UserId == id));
        _logger.Debug($"User ID{id} successfully got");
        return res;
    }

    public void Unban(int id)
    {
        _logger.Debug($"Attempt to unban a user {id}");
        var recordToRemove = _context.Banned.FirstOrDefault(b => b.UserId == id);
        if (recordToRemove != null)
        {
            _context.Banned.Remove(recordToRemove);
            _context.SaveChanges();
        }
        _logger.Debug($"User {id} successfully unbanned");
    }

    public void UnbanByBanId(int user_id, int ban_id)
    {
        _logger.Debug($"Attempt to unban a user {user_id}");
        var recordToRemove = _context.Banned.FirstOrDefault(b => b.UserId == user_id && b.Id == ban_id);
        if (recordToRemove != null)
        {
            _context.Banned.Remove(recordToRemove);
            _context.SaveChanges();
        }
        _logger.Debug($"User {user_id} successfully unbanned");
    }

    public void BanMarks(int id, bool isBlock)
    {
        _logger.Debug($"Attempt to edit marksban {isBlock} a user {id}");
        var user = _context.Users.FirstOrDefault(b => b.Id == id);
        if (user != null)
        {
            user.BlockMarks = isBlock;
            _context.SaveChanges();
        }
        _logger.Debug($"User {id} successfully edited marksban");
    }

    public void UpdateToken(UserModel model)
    {
        _logger.Debug($"Attempt to update token");
        var scheme = _context.Users.FirstOrDefault(u => u.Id == model.Id);
        if (scheme is not null)
        {
            scheme.RefreshToken = model.RefreshToken;
            _context.SaveChanges();
        }
        _logger.Debug($"Token successfully updated");
    }

    public void ChangePassword(int id, string password)
    {
        _logger.Debug($"Attempt to update token");
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user is not null)
        {
            user.Password = password;
            _context.SaveChanges();
        }
        _logger.Debug($"Token successfully updated");
    }
}
