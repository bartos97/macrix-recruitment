using Microsoft.EntityFrameworkCore;

namespace macrix_api.Models;

public class PeopleContext : DbContext
{
    public DbSet<PersonEntity> peopleEntities { get; set; } = null!;
    public string DbPath { get; }

    public PeopleContext(DbContextOptions<PeopleContext> options) : base(options)
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = System.IO.Path.Join(path, "macrix_api_people.db");
    }

    public PeopleContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonEntity>().Property(b => b.CreatedTimestamp)
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<PersonEntity>().Property(b => b.LastUpdateTimestamp)
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }

    public override int SaveChanges()
    {
        SetupTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetupTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetupTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is PersonEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((PersonEntity)entityEntry.Entity).LastUpdateTimestamp = DateTime.UtcNow;
            if (entityEntry.State == EntityState.Added)
            {
                ((PersonEntity)entityEntry.Entity).CreatedTimestamp = DateTime.UtcNow;
            }
        }
    }
}
