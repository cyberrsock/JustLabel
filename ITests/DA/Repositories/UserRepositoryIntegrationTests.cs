using Xunit;
using JustLabel.Data.Models;
using JustLabel.Repositories;
using IntegrationTests.Data;
using IntegrationTests.Factories;

namespace IntegrationTests.Repositories;

[Collection("Test Database")]
public class UserRepositoryIntegrationTests : BaseRepositoryIntegrationTests
{
    private readonly UserRepository _userRepository;

    public UserRepositoryIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _userRepository = new(Fixture.CreateContext());
    }

    private JustLabel.Data.AppDbContext Initialize()
    {
        var context = Fixture.CreateContext();
        return context;
    }

    [Fact]
    public void TestAddUserInEmptyTable()
    {
        using var context = Initialize();

        // Arrange
        var user = UserModelFactory.Create(1, "testuser", "test@example.com", "password", "salt", "token", false, false);

        // Act
        _userRepository.Add(user);

        // Assert
        var users = (from u in context.Users select u).ToList();
        Assert.Single(users);
        Assert.Equal(user.Id, users[0].Id);
        Assert.Equal(user.Username, users[0].Username);
        Assert.Equal(user.Email, users[0].Email);
    }

    [Fact]
    public void TestAddUserInNonEmptyTable()
    {
        using var context = Initialize();

        // Arrange
        var user1 = UserDbModelFactory.Create(1, "testuser1", "test1@example.com", "password", "salt", "token", false, false);
        var user2 = UserModelFactory.Create(2, "testuser2", "test2@example.com", "password", "salt", "token", false, false);

        context.Users.Add(user1);
        context.SaveChanges();

        // Act
        _userRepository.Add(user2);

        // Assert
        var users = (from u in context.Users select u).ToList();
        Assert.Equal(2, users.Count);
        Assert.Equal(user2.Id, users[1].Id);
        Assert.Equal(user2.Username, users[1].Username);
        Assert.Equal(user2.Email, users[1].Email);
    }

    [Fact]
    public void TestBanUser()
    {
        using var context = Initialize();

        // Arrange
        var user1 = UserDbModelFactory.Create(1, "testuser1", "test1@example.com", "password", "salt", "token", true, false);
        var user2 = UserDbModelFactory.Create(2, "testuser2", "test2@example.com", "password", "salt", "token", false, false);
        context.Users.Add(user1);
        context.Users.Add(user2);
        context.SaveChanges();

        var banned = BannedModelFactory.Create(1, 2, 1, "Spamming", DateTime.Now);

        // Act
        _userRepository.Ban(banned);

        // Assert
        var bannedUsers = (from u in context.Banned select u).ToList();
        Assert.Single(bannedUsers);
        Assert.Equal(banned.UserId, bannedUsers[0].UserId);
        Assert.Equal(banned.AdminId, bannedUsers[0].AdminId);
        Assert.Equal(banned.Reason, bannedUsers[0].Reason);
    }

    [Fact]
    public void TestDoubleBanUser()
    {
        using var context = Initialize();

        // Arrange
        var user1 = UserDbModelFactory.Create(1, "testuser1", "test1@example.com", "password", "salt", "token", true, false);
        var user2 = UserDbModelFactory.Create(2, "testuser2", "test2@example.com", "password", "salt", "token", false, false);
        var user3 = UserDbModelFactory.Create(3, "testuser3", "test3@example.com", "password", "salt", "token", false, false);
        context.Users.Add(user1);
        context.Users.Add(user2);
        context.Users.Add(user3);

        var banned1 = BannedDbModelFactory.Create(1, 2, 1, "Spamming", DateTime.Now);
        var banned2 = BannedModelFactory.Create(2, 3, 1, "Bad user", DateTime.Now);

        context.Banned.Add(banned1);
        context.SaveChanges();

        // Act
        _userRepository.Ban(banned2);

        // Assert
        var bannedUsers = (from u in context.Banned select u).ToList();
        Assert.Equal(2, bannedUsers.Count);
        Assert.Equal(banned2.UserId, bannedUsers[1].UserId);
        Assert.Equal(banned2.AdminId, bannedUsers[1].AdminId);
        Assert.Equal(banned2.Reason, bannedUsers[1].Reason);
    }

    [Fact]
    public void TestGetUserByEmail()
    {
        using var context = Initialize();

        // Arrange
        var userDbModel = UserDbModelFactory.Create(1, "testuser", "test@example.com", "password", "salt", "token", false, false);
        context.Users.Add(userDbModel);
        context.SaveChanges();

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
        using var context = Initialize();

        // Arrange

        // Act
        var resultUser = _userRepository.GetUserByEmail("nonexistent@example.com");

        // Assert
        Assert.Null(resultUser);
    }

    [Fact]
    public void TestGetUserById()
    {
        using var context = Initialize();

        // Arrange
        var userDbModel = UserDbModelFactory.Create(123, "testuser", "test@example.com", "password", "salt", "token", false, false);
        context.Users.Add(userDbModel);
        context.SaveChanges();

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
        using var context = Initialize();

        // Arrange

        // Act
        var resultUser = _userRepository.GetUserById(1);

        // Assert
        Assert.Null(resultUser);
    }

    [Fact]
    public void TestGetUserByUsername()
    {
        using var context = Initialize();

        // Arrange
        var userDbModel = UserDbModelFactory.Create(1, "testuser", "test@example.com", "password", "salt", "token", false, false);
        context.Users.Add(userDbModel);
        context.SaveChanges();

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
        using var context = Initialize();

        // Arrange

        // Act
        var resultUser = _userRepository.GetUserByUsername("nonexistentuser");

        // Assert
        Assert.Null(resultUser);
    }

    [Fact]
    public void TestGetAllUsers()
    {
        using var context = Initialize();

        // Arrange
        var userDbModel1 = UserDbModelFactory.Create(1, "user1", "user1@example.com", "password", "salt", "token", false, false);
        var userDbModel2 = UserDbModelFactory.Create(2, "user2", "admin@example.com", "password", "salt", "token", false, false);
        context.Users.Add(userDbModel1);
        context.Users.Add(userDbModel2);
        context.SaveChanges();

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
        using var context = Initialize();

        // Arrange
        var userDbModel1 = UserDbModelFactory.Create(1, "user1", "user1@example.com", "password", "salt", "token", false, false);
        var userDbModel2 = UserDbModelFactory.Create(2, "admin", "admin@example.com", "password", "salt", "token", true, false);
        context.Users.Add(userDbModel1);
        context.Users.Add(userDbModel2);
        context.SaveChanges();

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
        using var context = Initialize();

        // Arrange
        var user1 = UserDbModelFactory.Create(1, "testuser1", "test1@example.com", "password", "salt", "token", true, false);
        var user2 = UserDbModelFactory.Create(2, "testuser2", "test2@example.com", "password", "salt", "token", false, false);
        context.Users.Add(user1);
        context.Users.Add(user2);

        var bannedDbModel = BannedDbModelFactory.Create(1, 2, 1, "Spamming", DateTime.Now);
        context.Banned.Add(bannedDbModel);
        context.SaveChanges();

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
        using var context = Initialize();

        // Arrange

        // Act
        var resultBan = _userRepository.IsBan(1);

        // Assert
        Assert.Null(resultBan);
    }

    [Fact]
    public void TestUnbanUser()
    {
        using var context = Initialize();

        // Arrange
        var user1 = UserDbModelFactory.Create(1, "testuser1", "test1@example.com", "password", "salt", "token", true, false);
        var user2 = UserDbModelFactory.Create(2, "testuser2", "test2@example.com", "password", "salt", "token", false, false);
        context.Users.Add(user1);
        context.Users.Add(user2);

        var bannedDbModel = BannedDbModelFactory.Create(1, 2, 1, "Spamming", DateTime.Now);
        context.Banned.Add(bannedDbModel);
        context.SaveChanges();

        // Act
        _userRepository.Unban(2);

        // Assert
        var bannedUsers = (from u in context.Banned select u).ToList();
        Assert.Empty(bannedUsers);
    }

    [Fact]
    public void TestUnbanUserNotFound()
    {
        using var context = Initialize();

        // Arrange
        var user1 = UserDbModelFactory.Create(1, "testuser1", "test1@example.com", "password", "salt", "token", true, false);
        var user2 = UserDbModelFactory.Create(2, "testuser2", "test2@example.com", "password", "salt", "token", false, false);
        context.Users.Add(user1);
        context.Users.Add(user2);

        var bannedDbModel = BannedDbModelFactory.Create(1, 2, 1, "Spamming", DateTime.Now);
        context.Banned.Add(bannedDbModel);
        context.SaveChanges();

        // Act
        _userRepository.Unban(1);

        // Assert
        var bannedUsers = (from u in context.Banned select u).ToList();
        Assert.Single(bannedUsers);
    }

    [Fact]
    public void TestUnbanUserByBanId()
    {
        using var context = Initialize();

        // Arrange
        var user1 = UserDbModelFactory.Create(1, "testuser1", "test1@example.com", "password", "salt", "token", true, false);
        var user2 = UserDbModelFactory.Create(2, "testuser2", "test2@example.com", "password", "salt", "token", false, false);
        context.Users.Add(user1);
        context.Users.Add(user2);

        var bannedDbModel = BannedDbModelFactory.Create(1, 2, 1, "Spamming", DateTime.Now);
        context.Banned.Add(bannedDbModel);
        context.SaveChanges();

        // Act
        _userRepository.UnbanByBanId(2, 1);

        // Assert
        var bannedUsers = (from u in context.Banned select u).ToList();
        Assert.Empty(bannedUsers);
    }

    [Fact]
    public void TestUnbanUserByBanIdNotFound()
    {
        using var context = Initialize();

        // Arrange
        var user1 = UserDbModelFactory.Create(1, "testuser1", "test1@example.com", "password", "salt", "token", true, false);
        var user2 = UserDbModelFactory.Create(2, "testuser2", "test2@example.com", "password", "salt", "token", false, false);
        context.Users.Add(user1);
        context.Users.Add(user2);

        var bannedDbModel = BannedDbModelFactory.Create(1, 2, 1, "Spamming", DateTime.Now);
        context.Banned.Add(bannedDbModel);
        context.SaveChanges();

        // Act
        _userRepository.UnbanByBanId(2, 2);

        // Assert
        var bannedUsers = (from u in context.Banned select u).ToList();
        Assert.Single(bannedUsers);
    }

    [Fact]
    public void TestUpdateTokenUserNotFound()
    {
        using var context = Initialize();

        // Arrange
        var updatedUserModel = UserModelFactory.Create(1, "testuser", "test@example.com", "password", "salt", "new_token", false, false);

        // Act
        _userRepository.UpdateToken(updatedUserModel);

        // Assert
        var users = (from u in context.Users select u).ToList();
        Assert.Empty(users);
    }
}

