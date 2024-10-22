using System.Text;
using JustLabel.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace E2ETests;

internal class Settings
{
    public Settings() { }

    public string SymmetricFuncTestKey = "";
}

public class PgWebApplicationFactory<T> : WebApplicationFactory<T>
    where T : class
{
    private const string ConnectionString =
        @"Host=localhost;Port=5544;Username=postgres;Password=123;Database=testdb";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var settings = new Settings
        {
            SymmetricFuncTestKey =
                "superkey12345678superkey12345678",
        };

        builder
            .UseEnvironment("Testing")
            .ConfigureServices(services =>
            {
                // services.PostConfigure<JwtBearerOptions>(
                //     JwtBearerDefaults.AuthenticationScheme,
                //     options =>
                //     {
                //         options.TokenValidationParameters =
                //             new TokenValidationParameters
                //             {
                //                 IssuerSigningKey = new SymmetricSecurityKey(
                //                     Encoding.UTF8.GetBytes(
                //                         settings.SymmetricFuncTestKey
                //                     )
                //                 ),
                //                 ValidateIssuerSigningKey = true,
                //                 ValidateIssuer = true,
                //                 ValidateAudience = true,
                //                 ValidateLifetime = true,
                //                 ValidIssuer = "http://localhost:9898",
                //                 ValidAudience = "http://localhost:3000",
                //             };
                //     }
                // );
            })
            .ConfigureTestServices(services =>
            {
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseNpgsql(ConnectionString)
                    .EnableSensitiveDataLogging()
                    .Options;

                services.AddScoped<AppDbContext>(
                    provider => new AppTestDbContext(options)
                );

                var serviceProvider = services.BuildServiceProvider();
                using var scope = serviceProvider.CreateScope();
                var scopedService = scope.ServiceProvider;
                var db = scopedService.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            });
    }
}
