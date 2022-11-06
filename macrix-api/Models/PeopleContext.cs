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
}
