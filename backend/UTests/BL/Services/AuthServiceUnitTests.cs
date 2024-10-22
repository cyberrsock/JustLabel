using Xunit;
using Moq;
using JustLabel.Models;
using JustLabel.Exceptions;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services;
using JustLabel.Utilities;
using UnitTests.Builders;

namespace UnitTests.Services;

public class AuthServiceUnitTests
{
    private readonly AuthService _authService;
    private readonly Mock<IUserRepository> _mockUserRepository = new();

    public AuthServiceUnitTests()
    {
        _authService = new AuthService(_mockUserRepository.Object);
    }

    [Fact]
    public void TestRegisterUserWithValidData()
    {
        // Arrange
        var user = new UserModelBuilder()
            .WithUsername("validUser")
            .WithPassword("strongPassword")
            .WithEmail("user@example.com")
            .Build();
        
        List<UserModel> users = [];

        var callCount = 0;
        _mockUserRepository.Setup(s => s.GetUserByUsername(user.Username))
            .Returns((string username) =>
            {
                callCount++;
                return callCount == 1 ? null : user;
            });
        _mockUserRepository.Setup(s => s.GetUserByEmail(user.Email)).Returns((UserModel)null);
        _mockUserRepository
            .Setup(s => s.Add(It.IsAny<UserModel>()))
            .Callback((UserModel d) => users.Add(d));

        // Act
        var authModel = _authService.Register(user);

        // Assert
        Assert.NotNull(authModel);
        Assert.NotNull(authModel.AccessToken);
        Assert.NotNull(authModel.RefreshToken);
        _mockUserRepository.Verify(s => s.Add(user), Times.Once);
    }

    [Fact]
    public void TestRegisterUserWithShortUsername()
    {
        // Arrange
        var user = new UserModelBuilder()
            .WithUsername("usr")
            .WithPassword("strongPassword")
            .Build();

        // Act
        var exception = Assert.Throws<FailedRegistrationException>(() => _authService.Register(user));

        // Assert
        Assert.Equal("The username length must be at least 5", exception.Message);
    }

    [Fact]
    public void TestLoginUserWithValidCredentials()
    {
        // Arrange
        var user = new UserModelBuilder()
            .WithUsername("existingUser")
            .WithPassword("correctPassword")
            .WithSalt("someSalt")
            .WithRefreshToken("someToken")
            .Build();

        var getUser = new UserModelBuilder()
            .WithUsername("existingUser")
            .WithPassword(SaltedHash.GenerateSaltedHash(user.Password, user.Salt))
            .WithSalt("someSalt")
            .WithRefreshToken("someToken")
            .Build();

        _mockUserRepository.Setup(s => s.GetUserByUsername(user.Username)).Returns(getUser);
        _mockUserRepository.Setup(s => s.IsBan(user.Id)).Returns((BannedModel)null);
        _mockUserRepository.Setup(s => s.UpdateToken(It.IsAny<UserModel>()));

        // Act
        var authModel = _authService.Login(user);

        // Assert
        Assert.NotNull(authModel);
        Assert.NotNull(authModel.AccessToken);
        Assert.NotNull(authModel.RefreshToken);
    }

    [Fact]
    public void TestLoginUserWithNonExistingUsername()
    {
        // Arrange
        var user = new UserModelBuilder()
            .WithUsername("nonExistentUser")
            .WithPassword("anyPassword")
            .Build();

        // Act
        var exception = Assert.Throws<UserNotExistsException>(() => _authService.Login(user));

        // Assert
        Assert.Equal("User with this username does not exist", exception.Message);
    }

    [Fact]
    public void TestUpdateTokenWithValidTokens()
    {
        // Arrange
        _mockUserRepository.Setup(s => s.UpdateToken(It.IsAny<UserModel>()));

        var access = JWTGenerator.GenerateAccessToken(1, false);
        var refresh = JWTGenerator.GenerateRefreshToken(access);

        var model = new AuthModelBuilder()
            .WithAccessToken(access)
            .WithRefreshToken(refresh)
            .Build();

        // Act
        var updatedTokens = _authService.UpdateToken(model);

        // Assert
        Assert.NotNull(updatedTokens);
        Assert.NotNull(updatedTokens.AccessToken);
        Assert.NotNull(updatedTokens.RefreshToken);
        _mockUserRepository.Verify(s => s.UpdateToken(It.IsAny<UserModel>()), Times.Once);
    }

    [Fact]
    public void TestUpdateTokenWithInvalidRefreshToken()
    {
        // Arrange
        var model = new AuthModelBuilder()
            .WithAccessToken("someAccessToken")
            .WithRefreshToken("invalidRefreshToken")
            .Build();

        // Act
        var exception = Assert.Throws<FailedLoginException>(() => _authService.UpdateToken(model));

        // Assert
        Assert.Equal("Wrong refresh token", exception.Message);
    }
}

