// using Xunit;
// using Moq;
// using JustLabel.Data;
// using JustLabel.Models;
// using JustLabel.Exceptions;
// using JustLabel.Repositories;
// using JustLabel.Services;
// using JustLabel.Utilities;
// using IntegrationTests.Data;
// using IntegrationTests.Builders;
// using IntegrationTests.Factories;

// namespace IntegrationTests.Services;

// [Collection("Test Database")]
// public class AuthServiceIntegrationTests : BaseServiceIntegrationTests
// {
//     private readonly AppDbContext _context;
//     private readonly AuthService _authService;
//     private readonly UserRepository _userRepository;

//     public AuthServiceIntegrationTests(DatabaseFixture fixture) : base(fixture)
//     {
//         _context = Fixture.CreateContext();
//         _userRepository = new(_context);
//         _authService = new AuthService(_userRepository);
//     }

//     private JustLabel.Data.AppDbContext Initialize()
//     {
//         var context = Fixture.CreateContext();
//         return context;
//     }

//     [Fact]
//     public void TestRegisterUserWithValidData()
//     {
//         using var context = Initialize();

//         // Arrange
//         var user = new UserModelBuilder()
//             .WithUsername("validUser")
//             .WithPassword("strongPassword")
//             .WithEmail("user@example.com")
//             .Build();

//         // Act
//         var authModel = _authService.Register(user);

//         // Assert
//         Assert.NotNull(authModel);
//         Assert.NotNull(authModel.AccessToken);
//         Assert.NotNull(authModel.RefreshToken);
//     }

//     [Fact]
//     public void TestRegisterUserWithShortUsername()
//     {
//         using var context = Initialize();

//         // Arrange
//         var user = new UserModelBuilder()
//             .WithUsername("usr")
//             .WithPassword("strongPassword")
//             .Build();

//         // Act
//         var exception = Assert.Throws<FailedRegistrationException>(() => _authService.Register(user));

//         // Assert
//         Assert.Equal("The username length must be at least 5", exception.Message);
//     }

//     [Fact]
//     public void TestLoginUserWithValidCredentials()
//     {
//         using var context = Initialize();

//         // Arrange
//         var user = new UserModelBuilder()
//             .WithUsername("existingUser")
//             .WithPassword("correctPassword")
//             .WithSalt("someSalt")
//             .WithRefreshToken("someToken")
//             .Build();

//         var getUser = new UserModelBuilder()
//             .WithUsername("existingUser")
//             .WithPassword(SaltedHash.GenerateSaltedHash(user.Password, user.Salt))
//             .WithSalt("someSalt")
//             .WithRefreshToken("someToken")
//             .Build();

//         context.Users.Add(UserDbModelFactory.Create(getUser));
//         context.SaveChanges();

//         // Act
//         var authModel = _authService.Login(user);

//         // Assert
//         Assert.NotNull(authModel);
//         Assert.NotNull(authModel.AccessToken);
//         Assert.NotNull(authModel.RefreshToken);
//     }

//     [Fact]
//     public void TestLoginUserWithNonExistingUsername()
//     {
//         using var context = Initialize();

//         // Arrange
//         var user = new UserModelBuilder()
//             .WithUsername("nonExistentUser")
//             .WithPassword("anyPassword")
//             .Build();

//         // Act
//         var exception = Assert.Throws<UserNotExistsException>(() => _authService.Login(user));

//         // Assert
//         Assert.Equal("User with this username does not exist", exception.Message);
//     }

//     [Fact]
//     public void TestUpdateTokenWithValidTokens()
//     {
//         using var context = Initialize();

//         // Arrange
//         var access = JWTGenerator.GenerateAccessToken(1, false);
//         var refresh = JWTGenerator.GenerateRefreshToken(access);

//         var model = new AuthModelBuilder()
//             .WithAccessToken(access)
//             .WithRefreshToken(refresh)
//             .Build();

//         // Act
//         var updatedTokens = _authService.UpdateToken(model);

//         // Assert
//         Assert.NotNull(updatedTokens);
//         Assert.NotNull(updatedTokens.AccessToken);
//         Assert.NotNull(updatedTokens.RefreshToken);
//     }

//     [Fact]
//     public void TestUpdateTokenWithInvalidRefreshToken()
//     {
//         using var context = Initialize();

//         // Arrange
//         var model = new AuthModelBuilder()
//             .WithAccessToken("someAccessToken")
//             .WithRefreshToken("invalidRefreshToken")
//             .Build();

//         // Act
//         var exception = Assert.Throws<FailedLoginException>(() => _authService.UpdateToken(model));

//         // Assert
//         Assert.Equal("Wrong refresh token", exception.Message);
//     }
// }

