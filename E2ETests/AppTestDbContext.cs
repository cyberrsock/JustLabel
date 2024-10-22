using JustLabel.Data;
using Microsoft.EntityFrameworkCore;

namespace E2ETests;

public class AppTestDbContext : AppDbContext
{
    public AppTestDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
