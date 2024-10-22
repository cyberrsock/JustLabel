using Serilog;
using JustLabel.Models;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services.Interfaces;
using JustLabel.Utilities;
using JustLabel.Exceptions;

namespace JustLabel.Services;

public class AuthService : IAuthService
{
    private IUserRepository _userRepository;
    private readonly ILogger _logger;
    
    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _logger = Log.ForContext<AuthService>();
    }

    public AuthModel Register(UserModel model)
    {
        _logger.Debug($"Attempt to register a user {model.Username}");

        if (model.Username.Length < 5)
        {
            _logger.Error($"User {model.Username} has a short username");
            throw new FailedRegistrationException("The username length must be at least 5");
        }

        if (model.Password.Length < 5)
        {
            _logger.Error($"User {model.Username} has a short password");
            throw new FailedRegistrationException("The password length must be at least 5");
        }

        var foundUser = _userRepository.GetUserByUsername(model.Username);
        if (foundUser is not null)
        {
            _logger.Error($"User {model.Username} with same username already exists");
            throw new UserExistsException("User with this username already registered");
        }

        foundUser = _userRepository.GetUserByEmail(model.Email);
        if (foundUser is not null)
        {
            _logger.Error($"User {model.Username} with same email already exists");
            throw new UserExistsException("User with this email already registered");
        }

        _logger.Debug($"Registration data of user {model.Username} is correct");

        model.Salt = SaltedHash.GenerateSalt();
        model.Password = SaltedHash.GenerateSaltedHash(model.Password, model.Salt);

        _logger.Debug($"SaltedHash for user {model.Username} has been generated");

        model.RefreshToken = "temp";
        _userRepository.Add(model);

        _logger.Debug($"User {model.Username} has been temporarily added to the database");

        foundUser = _userRepository.GetUserByUsername(model.Username);
        string accessToken = JWTGenerator.GenerateAccessToken(foundUser.Id, foundUser.IsAdmin);
        string refreshToken = JWTGenerator.GenerateRefreshToken(accessToken);
        
        _logger.Debug($"JWT for user {model.Username} has been generated");

        foundUser.RefreshToken = refreshToken;

        _userRepository.UpdateToken(foundUser);

        _logger.Information($"New user: id${model.Id} ${model.Username}");

        _logger.Debug($"User {model.Username} successfully registered");

        return new() {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public AuthModel Login(UserModel model)
    {
        _logger.Debug($"Attempt to login a user {model.Username}");

        var foundUser = _userRepository.GetUserByUsername(model.Username);
        if (foundUser is null)
        {
            _logger.Error($"User {model.Username} does not exist");
            throw new UserNotExistsException("User with this username does not exist");
        }

        var foundBan = _userRepository.IsBan(foundUser.Id);
        if (foundBan is not null)
        {
            _logger.Error($"User {model.Username} is banned");
            throw new BannedUserException("The user is banned. Reason: '" + foundBan.Reason + "'. Date: " + foundBan.BanDatetime);
        }

        if (!SaltedHash.VerifySaltedHash(model.Password, foundUser.Salt, foundUser.Password))
        {
            _logger.Error($"User {model.Username} enters the wrong password");
            throw new FailedLoginException("Wrong password");
        }

        _logger.Debug($"Login data of user {model.Username} is correct");
        
        string accessToken = JWTGenerator.GenerateAccessToken(foundUser.Id, foundUser.IsAdmin);
        string refreshToken = JWTGenerator.GenerateRefreshToken(accessToken);

        foundUser.RefreshToken = refreshToken;

        _userRepository.UpdateToken(foundUser);

        _logger.Debug($"User {model.Username} successfully login");

        return new() {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public AuthModel UpdateToken(AuthModel model)
    {
        _logger.Debug("Attempt to update token");
        
        if (!JWTGenerator.ValidateRefreshToken(model.RefreshToken, model.AccessToken))
        {
            _logger.Error($"Wrong refresh token: {model.RefreshToken}");
            throw new FailedLoginException("Wrong refresh token");
        }

        int id = JWTGenerator.ValidateAccessToken(model.AccessToken, false);
        if (id < 0)
        {
            _logger.Error($"Wrong access token: {id} {model.AccessToken}");
            throw new FailedLoginException("Wrong access token");
        }

        string accessToken = JWTGenerator.GenerateAccessToken(id, false);
        string refreshToken = JWTGenerator.GenerateRefreshToken(accessToken);

        _logger.Debug("Token was generated");

        UserModel user = new() {
            Id = id,
            RefreshToken = refreshToken
        };

        _userRepository.UpdateToken(user);

        _logger.Debug("Token was successfully updated");

        return new() {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
