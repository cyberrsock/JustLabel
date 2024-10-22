using JustLabel.DataMongoDb.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace JustLabel.DataMongoDb;

public class AppDbContextMongoDb: DbContext
{
    public AppDbContextMongoDb(DbContextOptions<AppDbContextMongoDb> options): base(options) {}

    public DbSet<DatasetDbModel> Datasets { get; set; }
    public DbSet<ImageDbModel> Images { get; set; }
    public DbSet<LabelDbModel> Labels { get; set; }
    public DbSet<MarkedDbModel> Marked { get; set; }
    public DbSet<ReportDbModel> Reports { get; set; }
    public DbSet<SchemeDbModel> Schemes { get; set; }
    public DbSet<LabelSchemeDbModel> LabelsSchemes { get; set; }
    public DbSet<MarkedAreaDbModel> MarkedAreas { get; set; }
    public DbSet<AreaDbModel> Areas { get; set; }
    public DbSet<UserDbModel> Users { get; set; }
    public DbSet<BannedDbModel> Banned { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DatasetDbModel>().ToCollection("Datasets");
        modelBuilder.Entity<ImageDbModel>().ToCollection("Images");
        modelBuilder.Entity<LabelDbModel>().ToCollection("Labels");
        modelBuilder.Entity<MarkedDbModel>().ToCollection("Marked");
        modelBuilder.Entity<ReportDbModel>().ToCollection("Report");
        modelBuilder.Entity<SchemeDbModel>().ToCollection("Schemes");
        modelBuilder.Entity<AreaDbModel>().ToCollection("Areas");
        modelBuilder.Entity<UserDbModel>().ToCollection("Users");
        modelBuilder.Entity<BannedDbModel>().ToCollection("Banned");
        modelBuilder.Entity<LabelSchemeDbModel>().ToCollection("LabelsSchemes");
        modelBuilder.Entity<MarkedAreaDbModel>().ToCollection("MarkedAreas");
    }
}
