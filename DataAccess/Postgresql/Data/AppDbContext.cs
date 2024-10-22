using JustLabel.Data.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace JustLabel.Data;

public class AppDbContext: DbContext
{
    public AppDbContext() { }
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options) {}
    protected AppDbContext(DbContextOptions options): base(options) {}

    public virtual DbSet<DatasetDbModel> Datasets { get; set; }
    public virtual DbSet<ImageDbModel> Images { get; set; }
    public virtual DbSet<LabelDbModel> Labels { get; set; }
    public virtual DbSet<MarkedDbModel> Marked { get; set; }
    public virtual DbSet<ReportDbModel> Reports { get; set; }
    public virtual DbSet<SchemeDbModel> Schemes { get; set; }
    public virtual DbSet<LabelSchemeDbModel> LabelsSchemes { get; set; }
    public virtual DbSet<MarkedAreaDbModel> MarkedAreas { get; set; }
    public virtual DbSet<AreaDbModel> Areas { get; set; }
    public virtual DbSet<UserDbModel> Users { get; set; }
    public virtual DbSet<BannedDbModel> Banned { get; set; }
    public virtual object AggregatedDbModels { get; set; }
    public virtual DbSet<AggregatedDbModel> Aggregated { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AggregatedDbModel>()
        .HasKey(e => new { e.ImageId, e.LabelId });

        modelBuilder
            .Entity<LabelDbModel>()
            .HasMany(l => l.Areas)
            .WithOne(a => a.Label)
            .HasForeignKey(a => a.LabelId);
        
        modelBuilder
            .Entity<LabelDbModel>()
            .HasMany(l => l.Schemes)
            .WithMany(s => s.Labels)
            .UsingEntity<LabelSchemeDbModel>(
                l => l.HasOne(l => l.Scheme).WithMany(s => s.LabelsSchemes),
                r => r.HasOne(s => s.Label).WithMany(l => l.LabelsSchemes)
            );

        modelBuilder
            .Entity<AreaDbModel>()
            .HasMany(a => a.Marks)
            .WithMany(m => m.Areas)
            .UsingEntity<MarkedAreaDbModel>(
                l => l.HasOne(a => a.Mark).WithMany(m => m.MarksAreas),
                r => r.HasOne(m => m.Area).WithMany(a => a.MarksAreas)
            );
        
        modelBuilder
            .Entity<SchemeDbModel>()
            .HasMany(s => s.Marks)
            .WithOne(m => m.Scheme)
            .HasForeignKey(m => m.SchemeId);
        
        modelBuilder
            .Entity<UserDbModel>()
            .HasMany(u => u.Marks)
            .WithOne(m => m.User)
            .HasForeignKey(m => m.CreatorId);

        modelBuilder
            .Entity<MarkedDbModel>()
            .HasMany(m => m.Reports)
            .WithOne(r => r.Mark)
            .HasForeignKey(r => r.MarkedId);

        modelBuilder
            .Entity<UserDbModel>()
            .HasMany(u => u.Reports)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.CreatorId);
        
        modelBuilder
            .Entity<ImageDbModel>()
            .HasMany(i => i.Marks)
            .WithOne(m => m.Image)
            .HasForeignKey(m => m.ImageId);
        
        modelBuilder
            .Entity<DatasetDbModel>()
            .HasMany(d => d.Images)
            .WithOne(i => i.Dataset)
            .HasForeignKey(i => i.DatasetId);
        
        modelBuilder
            .Entity<UserDbModel>()
            .HasMany(u => u.Schemes)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.CreatorId);
        
        modelBuilder
            .Entity<UserDbModel>()
            .HasMany(u => u.Datasets)
            .WithOne(d => d.User)
            .HasForeignKey(d => d.CreatorId);
        
        modelBuilder
            .Entity<UserDbModel>()
            .HasMany(u => u.Banned)
            .WithOne(b => b.User)
            .HasForeignKey(b => b.UserId);
        
        modelBuilder
            .Entity<UserDbModel>()
            .HasMany(u => u.BannedBy)
            .WithOne(b => b.Admin)
            .HasForeignKey(b => b.AdminId);
    }
}
