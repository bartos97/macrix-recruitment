using Microsoft.EntityFrameworkCore;

namespace macrix_api.Models;

public class PeopleDbContext : DbContext, IPeopleDbContext
{
    public virtual DbSet<PersonEntity> PeopleEntities { get; set; }
    public string DbPath { get; }

    public PeopleDbContext(DbContextOptions<PeopleDbContext> options) : base(options)
    {
        var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        DbPath = System.IO.Path.Join(path, "macrix_api_people.db");
    }

    public PeopleDbContext() { }

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

    public async Task<IEnumerable<PersonEntity>> GetAllAsync()
    {
        return await PeopleEntities.OrderBy(x => x.CreatedTimestamp).ToListAsync();
    }

    public async Task<PersonEntity?> GetOneAsync(long id)
    {
        return await PeopleEntities.FindAsync(id);
    }

    public async Task ModifyOneAsync(PersonEntity entity)
    {
        Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync();
    }

    public async Task AddOneAsync(PersonEntity entity)
    {
        PeopleEntities.Add(entity);
        await SaveChangesAsync();
    }

    public async Task<bool> DeleteOneAsync(long id)
    {
        var personEntity = await PeopleEntities.FindAsync(id);
        if (personEntity == null)
        {
            return false;
        }
        PeopleEntities.Remove(personEntity);
        await SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<PersonEntity>> BatchInsertUpdateAsync(IEnumerable<PersonEntity> entities)
    {
        foreach (var item in entities)
        {
            bool isUpdate = item.id > 0;
            Entry(item).State = isUpdate ? EntityState.Modified : EntityState.Added;
        }
        var newEntities = entities.Where(x => x.id == 0).ToList();
        await SaveChangesAsync();
        return newEntities;
    }

    public bool EntityExists(long id)
    {
        return (PeopleEntities?.Any(e => e.id == id)).GetValueOrDefault();
    }
}
