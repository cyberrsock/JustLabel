using System.Text;
using JustLabel.Data;
using JustLabel.Data.Models;
using JustLabel.Models;
using JustLabel.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace E2ETests;

public class PgWebApplicationFactory<T> : WebApplicationFactory<T>
    where T : class
{
    public string jwtToken = "";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        string host = Environment.GetEnvironmentVariable("POSTGRESQL_HOST")!;
        string port = Environment.GetEnvironmentVariable("POSTGRESQL_PORT")!;
        string username = Environment.GetEnvironmentVariable("POSTGRESQL_USERNAME")!;
        string password = Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD")!;
        string database = Environment.GetEnvironmentVariable("POSTGRESQL_DATABASE")!;
        string ConnectionString = $"Host={host};Port={port};Database={database};User Id={username};Password={password};";
        builder
            .UseEnvironment("Testing")
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

                db.Datasets.RemoveRange(db.Datasets);
                db.Images.RemoveRange(db.Images);
                db.Labels.RemoveRange(db.Labels);
                db.Marked.RemoveRange(db.Marked);
                db.Reports.RemoveRange(db.Reports);
                db.Schemes.RemoveRange(db.Schemes);
                db.LabelsSchemes.RemoveRange(db.LabelsSchemes);
                db.MarkedAreas.RemoveRange(db.MarkedAreas);
                db.Areas.RemoveRange(db.Areas);
                db.Users.RemoveRange(db.Users);
                db.Banned.RemoveRange(db.Banned);

                var Salt = SaltedHash.GenerateSalt();
                var Password = SaltedHash.GenerateSaltedHash("test123", Salt);


                jwtToken = JWTGenerator.GenerateAccessToken(1, false);
                db.Users.Add(new UserDbModel() {
                    Id = 1,
                    Username = "test123",
                    Email = "test123",
                    Password = Password,
                    Salt = Salt,
                    RefreshToken = JWTGenerator.GenerateRefreshToken(jwtToken)
                });

                db.SaveChanges();

            });
    }
}
