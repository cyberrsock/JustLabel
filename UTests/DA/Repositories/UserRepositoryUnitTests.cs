using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using UnitTests.Data;
using UnitTests.Factories;

namespace UnitTests.Repositories;

public class UserRepositoryUnitTests
{
    private readonly UserRepository _userRepository;
    private readonly MockDbContextFactory _mockFactory;

    public UserRepositoryUnitTests()
    {
        _mockFactory = new MockDbContextFactory();
        _userRepository = new UserRepository(_mockFactory.MockContext.Object);
    }

    [Fact]
    public void TestAddUserInEmptyTable()
    {
        // Arrange
        var user = UserModelFactory.Create(1, "testuser", "test@example.com", "password", "salt", "token", false, false);
        List<UserDbModel> users = [];
        _mockFactory.SetUserList(users);

        // Act
        _userRepository.Add(user);

        // Assert
        Assert.Single(users);
        Assert.Equal(user.Id, users[0].Id);
        Assert.Equal(user.Username, users[0].Username);
        Assert.Equal(user.Email, users[0].Email);
    }

    [Fact]
    public void TestAddUserInNonEmptyTable()
    {
        // Arrange
        var user1 = UserDbModelFactory.Create(1, "testuser1", "test1@example.com", "password", "salt", "token", false, false);
        var user2 = UserModelFactory.Create(2, "testuser2", "test2@example.com", "password", "salt", "token", false, false);

        List<UserDbModel> users = [user1];
        _mockFactory.SetUserList(users);

        // Act
        _userRepository.Add(user2);

        // Assert
        Assert.Equal(2, users.Count);
        Assert.Equal(user2.Id, users[1].Id);
        Assert.Equal(user2.Username, users[1].Username);
        Assert.Equal(user2.Email, users[1].Email);
    }

    [Fact]
    public void TestBanUser()
    {
        // Arrange
        var banned = BannedModelFactory.Create(1, 2, 1, "Spamming", DateTime.Now);
        List<BannedDbModel> bannedUsers = [];
        _mockFactory.SetBannedList(bannedUsers);

        // Act
        _userRepository.Ban(banned);

        // Assert
        Assert.Single(bannedUsers);
        Assert.Equal(banned.UserId, bannedUsers[0].UserId);
        Assert.Equal(banned.AdminId, bannedUsers[0].AdminId);
        Assert.Equal(banned.Reason, bannedUsers[0].Reason);
    }

    [Fact]
    public void TestDoubleBanUser()
    {
        // Arrange
        var banned1 = BannedDbModelFactory.Create(1, 2, 1, "Spamming", DateTime.Now);
        var banned2 = BannedModelFactory.Create(1, 2, 3, "Bad user", DateTime.Now);
        List<BannedDbModel> bannedUsers = [banned1];
        _mockFactory.SetBannedList(bannedUsers);

        // Act
        _userRepository.Ban(banned2);

        // Assert
        Assert.Equal(2, bannedUsers.Count);
        Assert.Equal(banned2.UserId, bannedUsers[1].UserId);
        Assert.Equal(banned2.AdminId, bannedUsers[1].AdminId);
        Assert.Equal(banned2.Reason, bannedUsers[1].Reason);
    }

    [Fact]
    public void TestGetUserByEmail()
    {
        // Arrange
        var userDbModel = UserDbModelFactory.Create(1, "testuser", "test@example.com", "password", "salt", "token", false, false);
        List<UserDbModel> users = [userDbModel];
        _mockFactory.SetUserList(users);

        // Act
        var resultUser = _userRepository.GetUserByEmail("test@example.com");

        // Assert
        Assert.NotNull(resultUser);
        Assert.Equal(userDbModel.Id, resultUser.Id);
        Assert.Equal(userDbModel.Username, resultUser.Username);
        Assert.Equal(userDbModel.Email, resultUser.Email);
    }

    [Fact]
    public void TestGetUserByEmailNotFound()
    {
        // Arrange
        List<UserDbModel> users = [];
        _mockFactory.SetUserList(users);

        // Act
        var resultUser = _userRepository.GetUserByEmail("nonexistent@example.com");

        // Assert
        Assert.Null(resultUser);
    }

    [Fact]
    public void TestGetUserById()
    {
        // Arrange
        var userDbModel = UserDbModelFactory.Create(123, "testuser", "test@example.com", "password", "salt", "token", false, false);
        List<UserDbModel> users = [userDbModel];
        _mockFactory.SetUserList(users);

        // Act
        var resultUser = _userRepository.GetUserById(123);

        // Assert
        Assert.NotNull(resultUser);
        Assert.Equal(userDbModel.Id, resultUser.Id);
        Assert.Equal(userDbModel.Username, resultUser.Username);
        Assert.Equal(userDbModel.Email, resultUser.Email);
    }

    [Fact]
    public void TestGetUserByIdNotFound()
    {
        // Arrange
        List<UserDbModel> users = [];
        _mockFactory.SetUserList(users);

        // Act
        var resultUser = _userRepository.GetUserById(1);

        // Assert
        Assert.Null(resultUser);
    }

    [Fact]
    public void TestGetUserByUsername()
    {
        // Arrange
        var userDbModel = UserDbModelFactory.Create(1, "testuser", "test@example.com", "password", "salt", "token", false, false);
        List<UserDbModel> users = [userDbModel];
        _mockFactory.SetUserList(users);

        // Act
        var resultUser = _userRepository.GetUserByUsername("testuser");

        // Assert
        Assert.NotNull(resultUser);
        Assert.Equal(userDbModel.Id, resultUser.Id);
        Assert.Equal(userDbModel.Username, resultUser.Username);
        Assert.Equal(userDbModel.Email, resultUser.Email);
    }

    [Fact]
    public void TestGetUserByUsernameNotFound()
    {
        // Arrange
        List<UserDbModel> users = [];
        _mockFactory.SetUserList(users);

        // Act
        var resultUser = _userRepository.GetUserByUsername("nonexistentuser");

        // Assert
        Assert.Null(resultUser);
    }

    [Fact]
    public void TestGetAllUsers()
    {
        // Arrange
        var userDbModel1 = UserDbModelFactory.Create(1, "user1", "user1@example.com", "password", "salt", "token", false, false);
        var userDbModel2 = UserDbModelFactory.Create(2, "user2", "admin@example.com", "password", "salt", "token", false, false);
        List<UserDbModel> users = [userDbModel1, userDbModel2];
        _mockFactory.SetUserList(users);

        // Act
        var allUsers = _userRepository.GetAll();

        // Assert
        Assert.Equal(2, allUsers.Count);
        Assert.Equal(userDbModel1.Id, allUsers[0].Id);
        Assert.Equal(userDbModel1.Username, allUsers[0].Username);
        Assert.Equal(userDbModel1.Email, allUsers[0].Email);
        Assert.False(allUsers[0].IsAdmin);
        Assert.Equal(userDbModel2.Id, allUsers[1].Id);
        Assert.Equal(userDbModel2.Username, allUsers[1].Username);
        Assert.Equal(userDbModel2.Email, allUsers[1].Email);
        Assert.False(allUsers[1].IsAdmin);
    }

    [Fact]
    public void TestGetAllUsersNoAdmin()
    {
        // Arrange
        var userDbModel1 = UserDbModelFactory.Create(1, "user1", "user1@example.com", "password", "salt", "token", false, false);
        var userDbModel2 = UserDbModelFactory.Create(2, "admin", "admin@example.com", "password", "salt", "token", true, false);
        List<UserDbModel> users = [userDbModel1, userDbModel2];
        _mockFactory.SetUserList(users);

        // Act
        var allUsers = _userRepository.GetAll();

        // Assert
        Assert.Single(allUsers);
        Assert.Equal(userDbModel1.Id, allUsers[0].Id);
        Assert.Equal(userDbModel1.Username, allUsers[0].Username);
        Assert.Equal(userDbModel1.Email, allUsers[0].Email);
        Assert.False(allUsers[0].IsAdmin);
    }

    [Fact]
    public void TestIsBan()
    {
        // Arrange
        var bannedDbModel = BannedDbModelFactory.Create(1, 2, 1, "Spamming", DateTime.Now);
        List<BannedDbModel> bannedUsers = [bannedDbModel];
        _mockFactory.SetBannedList(bannedUsers);

        // Act
        var resultBan = _userRepository.IsBan(2);

        // Assert
        Assert.NotNull(resultBan);
        Assert.Equal(bannedDbModel.Id, resultBan.Id);
        Assert.Equal(bannedDbModel.UserId, resultBan.UserId);
        Assert.Equal(bannedDbModel.AdminId, resultBan.AdminId);
        Assert.Equal(bannedDbModel.Reason, resultBan.Reason);
    }

    [Fact]
    public void TestIsBanNotFound()
    {
        // Arrange
        List<BannedDbModel> bannedUsers = [];
        _mockFactory.SetBannedList(bannedUsers);

        // Act
        var resultBan = _userRepository.IsBan(1);

        // Assert
        Assert.Null(resultBan);
    }

    [Fact]
    public void TestUnbanUser()
    {
        // Arrange
        var bannedDbModel = BannedDbModelFactory.Create(1, 2, 1, "Spamming", DateTime.Now);
        List<BannedDbModel> bannedUsers = [bannedDbModel];
        _mockFactory.SetBannedList(bannedUsers);

        // Act
        _userRepository.Unban(2);

        // Assert
        Assert.Empty(bannedUsers);
    }

    [Fact]
    public void TestUnbanUserNotFound()
    {
        // Arrange
        var bannedDbModel = BannedDbModelFactory.Create(1, 2, 1, "Spamming", DateTime.Now);
        List<BannedDbModel> bannedUsers = [bannedDbModel];
        _mockFactory.SetBannedList(bannedUsers);

        // Act
        _userRepository.Unban(1);

        // Assert
        Assert.Single(bannedUsers);
    }

    [Fact]
    public void TestUnbanUserByBanId()
    {
        // Arrange
        var bannedDbModel = BannedDbModelFactory.Create(1, 2, 1, "Spamming", DateTime.Now);
        List<BannedDbModel> bannedUsers = [bannedDbModel];
        _mockFactory.SetBannedList(bannedUsers);

        // Act
        _userRepository.UnbanByBanId(2, 1);

        // Assert
        Assert.Empty(bannedUsers);
    }

    [Fact]
    public void TestUnbanUserByBanIdNotFound()
    {
        // Arrange
        var bannedDbModel = BannedDbModelFactory.Create(1, 2, 1, "Spamming", DateTime.Now);
        List<BannedDbModel> bannedUsers = [bannedDbModel];
        _mockFactory.SetBannedList(bannedUsers);

        // Act
        _userRepository.UnbanByBanId(2, 2);

        // Assert
        Assert.Single(bannedUsers);
    }

    [Fact]
    public void TestBanMarks()
    {
        // Arrange
        var userDbModel = UserDbModelFactory.Create(1, "testuser", "test@example.com", "password", "salt", "token", false, false);
        List<UserDbModel> users = [userDbModel];
        _mockFactory.SetUserList(users);

        // Act
        _userRepository.BanMarks(1, true);

        // Assert
        Assert.True(users[0].BlockMarks);
    }

    [Fact]
    public void TestUnbanMarks()
    {
        // Arrange
        var userDbModel = UserDbModelFactory.Create(1, "testuser", "test@example.com", "password", "salt", "token", false, true);
        List<UserDbModel> users = [userDbModel];
        _mockFactory.SetUserList(users);

        // Act
        _userRepository.BanMarks(1, false);

        // Assert
        Assert.False(users[0].BlockMarks);
    }

    [Fact]
    public void TestUpdateToken()
    {
        // Arrange
        var userDbModel = UserDbModelFactory.Create(1, "testuser", "test@example.com", "password", "salt", "old_token", false, false);
        List<UserDbModel> users = [userDbModel];
        _mockFactory.SetUserList(users);

        var updatedUserModel = UserModelFactory.Create(1, "testuser", "test@example.com", "password", "salt", "new_token", false, false);

        // Act
        _userRepository.UpdateToken(updatedUserModel);

        // Assert
        Assert.Equal(userDbModel.Id, users[0].Id);
        Assert.Equal(updatedUserModel.Id, users[0].Id);
        Assert.Equal("new_token", users[0].RefreshToken);
    }

    [Fact]
    public void TestUpdateTokenUserNotFound()
    {
        // Arrange
        List<UserDbModel> users = [];
        _mockFactory.SetUserList(users);

        var updatedUserModel = UserModelFactory.Create(1, "testuser", "test@example.com", "password", "salt", "new_token", false, false);

        // Act
        _userRepository.UpdateToken(updatedUserModel);

        // Assert
        Assert.Empty(users);
    }
}

