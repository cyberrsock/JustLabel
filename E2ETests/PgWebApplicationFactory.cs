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

    private const string ConnectionString =
        @"Host=localhost;Port=5432;Username=postgres;Password=123;Database=testdb";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
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
