using Xunit;
using Moq;
using JustLabel.Data;
using JustLabel.Models;
using JustLabel.Exceptions;
using JustLabel.Repositories;
using JustLabel.Services;
using IntegrationTests.Data;
using IntegrationTests.Builders;

namespace IntegrationTests.Services;

[Collection("Test Database")]
public class UserServiceIntegrationTests : BaseServiceIntegrationTests
{
    private readonly AppDbContext _context;
    private readonly UserService _userService;
    private readonly UserRepository _userRepository;

    public UserServiceIntegrationTests(DatabaseFixture fixture) : base(fixture)
    {
        _context = Fixture.CreateContext();
        _userRepository = new UserRepository(_context);
        _userService = new UserService(_userRepository);
    }

    private JustLabel.Data.AppDbContext Initialize()
    {
        var context = Fixture.CreateContext();
        return context;
    }

    [Fact]
    public void GetUserByIDExistingUser()
    {
        var context = Fixture.CreateContext();

        // Arrange
        int userId = 1;
        var user = new UserDbModelBuilder()
            .WithId(userId)
            .WithUsername("TestUser")
            .Build();
        context.Users.Add(user);
        context.SaveChanges();

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
        var context = Fixture.CreateContext();

        // Arrange
        int userId = 1;

        // Act
        var exception = Assert.Throws<UserNotExistsException>(() => _userService.GetUserByID(userId));

        // Assert
        Assert.Equal("User with this id does not exist", exception.Message);
    }

    [Fact]
    public void GetUserByIDsExistingUsers()
    {
        var context = Fixture.CreateContext();

        // Arrange
        var userIds = new List<int> { 1, 2 };
        var user1 = new UserDbModelBuilder().WithId(1).WithUsername("User1").Build();
        var user2 = new UserDbModelBuilder().WithId(2).WithUsername("User2").Build();
        context.Users.Add(user1);
        context.Users.Add(user2);
        context.SaveChanges();

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
        var context = Fixture.CreateContext();

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
        var context = Fixture.CreateContext();

        // Arrange
        var bannedModel = new BannedModelBuilder()
            .WithUserId(1)
            .WithAdminId(2)
            .WithReason("Violation of rules")
            .Build();

        var adminUser = new UserDbModelBuilder()
            .WithId(2)
            .WithIsAdmin(true)
            .Build();

        var targetUser = new UserDbModelBuilder()
            .WithId(1)
            .WithIsAdmin(false)
            .Build();

        context.Users.Add(adminUser);
        context.Users.Add(targetUser);
        context.SaveChanges();

        // Act
        _userService.Ban(bannedModel);

        // Assert
        var bans = (from b in context.Banned select b).ToList();
        Assert.Single(bans);
    }

    [Fact]
    public void BanUserDoesNotExist()
    {
        var context = Fixture.CreateContext();

        // Arrange
        var bannedModel = new BannedModelBuilder().WithUserId(1).WithAdminId(2).Build();

        // Act
        var exception = Assert.Throws<UserNotExistsException>(() => _userService.Ban(bannedModel));

        // Assert
        Assert.Equal("User with this id does not exist", exception.Message);
    }

    [Fact]
    public void GetUsersAdminUser()
    {
        var context = Fixture.CreateContext();

        // Arrange
        var adminId = 1;
        var adminUser = new UserDbModelBuilder()
            .WithId(adminId)
            .WithIsAdmin(true)
            .Build();

        var user1 = new UserDbModelBuilder().WithId(2).WithUsername("User2").Build();
        var user2 = new UserDbModelBuilder().WithId(3).WithUsername("User3").Build();
        context.Users.Add(adminUser);
        context.Users.Add(user1);
        context.Users.Add(user2);
        context.SaveChanges();

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
        var context = Fixture.CreateContext();

        // Arrange
        var adminId = 1;
        var nonAdminUser = new UserDbModelBuilder()
            .WithId(adminId)
            .WithIsAdmin(false)
            .Build();
        context.Users.Add(nonAdminUser);
        context.SaveChanges();

        // Act
        var exception = Assert.Throws<AdminUserException>(() => _userService.GetUsers(adminId));

        // Assert
        Assert.Equal("The user with AdminId is not admin", exception.Message);
    }

    [Fact]
    public void UnbanUserIsBanned()
    {
        var context = Fixture.CreateContext();

        // Arrange
        int userId = 1;
        var nonAdminUser = new UserDbModelBuilder()
            .WithId(1)
            .WithIsAdmin(false)
            .Build();
        var adminUser = new UserDbModelBuilder()
            .WithId(2)
            .WithIsAdmin(true)
            .Build();

        context.Users.Add(adminUser);
        context.Users.Add(nonAdminUser);
        var bannedEntry = new BannedDbModelBuilder().WithUserId(userId).WithAdminId(2).Build();
        context.Banned.Add(bannedEntry);
        context.SaveChanges();

        // Act
        _userService.Unban(userId);

        // Assert
        var bans = (from b in context.Banned select b).ToList();
        Assert.Empty(bans);
    }

    [Fact]
    public void UnbanUserIsNotBanned()
    {
        var context = Fixture.CreateContext();

        // Arrange
        int userId = 1;

        // Act
        var exception = Assert.Throws<UnbannedUserException>(() => _userService.Unban(userId));

        // Assert
        Assert.Equal("The user is not banned", exception.Message);
    }

    [Fact]
    public void UnbanByBanIdUserIsBanned()
    {
        var context = Fixture.CreateContext();

        // Arrange
        int userId = 1;
        int banId = 10;
        var nonAdminUser = new UserDbModelBuilder()
            .WithId(1)
            .WithIsAdmin(false)
            .Build();
        var adminUser = new UserDbModelBuilder()
            .WithId(2)
            .WithIsAdmin(true)
            .Build();

        context.Users.Add(adminUser);
        context.Users.Add(nonAdminUser);
        var bannedEntry = new BannedDbModelBuilder().WithId(banId).WithUserId(userId).WithAdminId(2).Build();
        context.Banned.Add(bannedEntry);
        context.SaveChanges();

        // Act
        _userService.UnbanByBanId(userId, banId);

        // Assert
        var bans = (from b in context.Banned select b).ToList();
        Assert.Empty(bans);
    }

    [Fact]
    public void UnbanByBanIdUserIsNotBanned()
    {
        var context = Fixture.CreateContext();

        // Arrange
        int userId = 1;
        int banId = 10;

        // Act
        var exception = Assert.Throws<UnbannedUserException>(() => _userService.UnbanByBanId(userId, banId));

        // Assert
        Assert.Equal("The user is not banned", exception.Message);
    }

    [Fact]
    public void BanMarksAdminIsNotAdmin()
    {
        var context = Fixture.CreateContext();

        // Arrange
        int userId = 1;
        int adminId = 2;
        var nonAdminUser = new UserDbModelBuilder()
            .WithId(adminId)
            .WithIsAdmin(false)
            .Build();
        context.Users.Add(nonAdminUser);
        context.SaveChanges();

        // Act
        var exception = Assert.Throws<AdminUserException>(() => _userService.BanMarks(userId, adminId, true));

        // Assert
        Assert.Equal("The user with AdminId is not admin", exception.Message);
    }

    [Fact]
    public void IsBannedUserIsBanned()
    {
        var context = Fixture.CreateContext();

        // Arrange
        int userId = 1;
        var nonAdminUser = new UserDbModelBuilder()
            .WithId(1)
            .WithIsAdmin(false)
            .Build();
        var adminUser = new UserDbModelBuilder()
            .WithId(2)
            .WithIsAdmin(true)
            .Build();

        context.Users.Add(adminUser);
        context.Users.Add(nonAdminUser);
        var bannedEntry = new BannedDbModelBuilder().WithId(10).WithUserId(userId).WithAdminId(2).Build();
        context.Banned.Add(bannedEntry);
        context.SaveChanges();

        // Act
        int result = _userService.IsBanned(userId);

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void IsBannedUserIsNotBanned()
    {
        var context = Fixture.CreateContext();

        // Arrange
        int userId = 1;

        // Act
        int result = _userService.IsBanned(userId);

        // Assert
        Assert.Equal(0, result);
    }
}
