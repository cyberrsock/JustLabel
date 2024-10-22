using Xunit;
using Moq;
using JustLabel.Models;
using JustLabel.Exceptions;
using JustLabel.Repositories.Interfaces;
using JustLabel.Services;
using UnitTests.Builders;

namespace UnitTests.Services;

public class UserServiceUnitTests
{
    private readonly UserService _userService;
    private readonly Mock<IUserRepository> _mockUserRepository = new();

    public UserServiceUnitTests()
    {
        _userService = new UserService(_mockUserRepository.Object);
    }

    [Fact]
    public void GetUserByIDExistingUser()
    {
        // Arrange
        int userId = 1;
        var user = new UserModelBuilder()
            .WithId(userId)
            .WithUsername("TestUser")
            .Build();

        _mockUserRepository.Setup(repo => repo.GetUserById(userId)).Returns(user);

        // Act
        var result = _userService.GetUserByID(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("TestUser", result.Username);
    }

    [Fact]
    public void GetUserByIDNonExistingUser()
    {
        // Arrange
        int userId = 1;
        _mockUserRepository.Setup(repo => repo.GetUserById(userId)).Returns((UserModel)null);

        // Act
        var exception = Assert.Throws<UserNotExistsException>(() => _userService.GetUserByID(userId));

        // Assert
        Assert.Equal("User with this id does not exist", exception.Message);
    }

    [Fact]
    public void GetUserByIDsExistingUsers()
    {
        // Arrange
        var userIds = new List<int> { 1, 2 };
        var users = new List<UserModel>
        {
            new UserModelBuilder().WithId(1).WithUsername("User1").Build(),
            new UserModelBuilder().WithId(2).WithUsername("User2").Build(),
        };

        _mockUserRepository.Setup(repo => repo.GetUserById(1)).Returns(users[0]);
        _mockUserRepository.Setup(repo => repo.GetUserById(2)).Returns(users[1]);

        // Act
        var result = _userService.GetUserByIDs(userIds);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("User1", result[0].Username);
        Assert.Equal("User2", result[1].Username);
    }

    [Fact]
    public void GetUserByIDsEmptyList()
    {
        // Arrange
        var userIds = new List<int>();

        // Act
        var result = _userService.GetUserByIDs(userIds);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void BanUserExists()
    {
        // Arrange
        var bannedModel = new BannedModelBuilder()
            .WithUserId(1)
            .WithAdminId(2)
            .WithReason("Violation of rules")
            .Build();

        var adminUser = new UserModelBuilder()
            .WithId(2)
            .WithIsAdmin(true)
            .Build();
        
        var targetUser = new UserModelBuilder()
            .WithId(1)
            .WithIsAdmin(false)
            .Build();

        _mockUserRepository.Setup(repo => repo.GetUserById(bannedModel.UserId)).Returns(targetUser);
        _mockUserRepository.Setup(repo => repo.GetUserById(bannedModel.AdminId)).Returns(adminUser);
        _mockUserRepository.Setup(repo => repo.IsBan(bannedModel.UserId)).Returns((BannedModel)null);

        // Act
        _userService.Ban(bannedModel);

        // Assert
        _mockUserRepository.Verify(repo => repo.Ban(bannedModel), Times.Once);
    }

    [Fact]
    public void BanUserDoesNotExist()
    {
        // Arrange
        var bannedModel = new BannedModelBuilder().WithUserId(1).WithAdminId(2).Build();
        
        _mockUserRepository.Setup(repo => repo.GetUserById(bannedModel.UserId)).Returns((UserModel)null);

        // Act
        var exception = Assert.Throws<UserNotExistsException>(() => _userService.Ban(bannedModel));

        // Assert
        Assert.Equal("User with this id does not exist", exception.Message);
    }

    [Fact]
    public void GetUsersAdminUser()
    {
        // Arrange
        var adminId = 1;
        var adminUser = new UserModelBuilder()
            .WithId(adminId)
            .WithIsAdmin(true)
            .Build();

        var users = new List<UserModel>
        {
            new UserModelBuilder().WithId(2).WithUsername("User2").Build(),
            new UserModelBuilder().WithId(3).WithUsername("User3").Build()
        };

        _mockUserRepository.Setup(repo => repo.GetUserById(adminId)).Returns(adminUser);
        _mockUserRepository.Setup(repo => repo.GetAll()).Returns(users);

        // Act
        var result = _userService.GetUsers(adminId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("User2", result[0].Username);
        Assert.Equal("User3", result[1].Username);
    }

    [Fact]
    public void GetUsersUserIsNotAdmin()
    {
        // Arrange
        var adminId = 1;
        var nonAdminUser = new UserModelBuilder()
            .WithId(adminId)
            .WithIsAdmin(false)
            .Build();

        _mockUserRepository.Setup(repo => repo.GetUserById(adminId)).Returns(nonAdminUser);

        // Act
        var exception = Assert.Throws<AdminUserException>(() => _userService.GetUsers(adminId));

        // Assert
        Assert.Equal("The user with AdminId is not admin", exception.Message);
    }

    [Fact]
    public void UnbanUserIsBanned()
    {
        // Arrange
        int userId = 1;
        var bannedEntry = new BannedModelBuilder().WithUserId(userId).Build();

        _mockUserRepository.Setup(repo => repo.IsBan(userId)).Returns(bannedEntry);

        // Act
        _userService.Unban(userId);

        // Assert
        _mockUserRepository.Verify(repo => repo.Unban(userId), Times.Once);
    }

    [Fact]
    public void UnbanUserIsNotBanned()
    {
        // Arrange
        int userId = 1;

        _mockUserRepository.Setup(repo => repo.IsBan(userId)).Returns((BannedModel)null);

        // Act
        var exception = Assert.Throws<UnbannedUserException>(() => _userService.Unban(userId));

        // Assert
        Assert.Equal("The user is not banned", exception.Message);
    }

    [Fact]
    public void UnbanByBanIdUserIsBanned()
    {
        // Arrange
        int userId = 1;
        int banId = 10;
        var bannedEntry = new BannedModelBuilder().WithUserId(userId).Build();

        _mockUserRepository.Setup(repo => repo.IsBan(userId)).Returns(bannedEntry);

        // Act
        _userService.UnbanByBanId(userId, banId);

        // Assert
        _mockUserRepository.Verify(repo => repo.UnbanByBanId(userId, banId), Times.Once);
    }

    [Fact]
    public void UnbanByBanIdUserIsNotBanned()
    {
        // Arrange
        int userId = 1;
        int banId = 10;

        _mockUserRepository.Setup(repo => repo.IsBan(userId)).Returns((BannedModel)null);

        // Act
        var exception = Assert.Throws<UnbannedUserException>(() => _userService.UnbanByBanId(userId, banId));

        // Assert
        Assert.Equal("The user is not banned", exception.Message);
    }

    [Fact]
    public void BanMarksUserIsAdmin()
    {
        // Arrange
        int userId = 1;
        int adminId = 2;
        bool isBlock = true;

        var adminUser = new UserModelBuilder()
            .WithId(adminId)
            .WithIsAdmin(true)
            .Build();

        // Set up the mock repository
        _mockUserRepository.Setup(repo => repo.GetUserById(adminId)).Returns(adminUser);

        // Act
        _userService.BanMarks(userId, adminId, isBlock);

        // Assert
        _mockUserRepository.Verify(repo => repo.BanMarks(userId, isBlock), Times.Once);
    }

    [Fact]
    public void BanMarksAdminIsNotAdmin()
    {
        // Arrange
        int userId = 1;
        int adminId = 2;
        var nonAdminUser = new UserModelBuilder()
            .WithId(adminId)
            .WithIsAdmin(false)
            .Build();

        _mockUserRepository.Setup(repo => repo.GetUserById(adminId)).Returns(nonAdminUser);

        // Act
        var exception = Assert.Throws<AdminUserException>(() => _userService.BanMarks(userId, adminId, true));

        // Assert
        Assert.Equal("The user with AdminId is not admin", exception.Message);
    }

    [Fact]
    public void IsBannedUserIsBanned()
    {
        // Arrange
        int userId = 1;
        var bannedEntry = new BannedModelBuilder().WithId(10).WithUserId(userId).Build();

        _mockUserRepository.Setup(repo => repo.IsBan(userId)).Returns(bannedEntry);

        // Act
        int result = _userService.IsBanned(userId);

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void IsBannedUserIsNotBanned()
    {
        // Arrange
        int userId = 1;

        _mockUserRepository.Setup(repo => repo.IsBan(userId)).Returns((BannedModel)null);

        // Act
        int result = _userService.IsBanned(userId);

        // Assert
        Assert.Equal(0, result);
    }
}
