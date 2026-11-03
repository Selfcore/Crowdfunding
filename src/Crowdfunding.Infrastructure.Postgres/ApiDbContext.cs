using Crowdfunding.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crowdfunding.Infrastructure.Postgres;

public class ApiDbContext : DbContext
{
    public DbSet<User> Users { get; private set; }
    public DbSet<Project> Projects { get; private set; }
    public DbSet<Reward> Rewards { get; private set; }
    public DbSet<Pledge> Pledges { get; private set; }

    public ApiDbContext(DbContextOptions<ApiDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiDbContext).Assembly);
    }
}