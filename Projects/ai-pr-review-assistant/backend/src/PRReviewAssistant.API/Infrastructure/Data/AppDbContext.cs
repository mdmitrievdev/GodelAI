using Microsoft.EntityFrameworkCore;
using PRReviewAssistant.API.Domain.Entities;

namespace PRReviewAssistant.API.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Finding> Findings => Set<Finding>();
    public DbSet<AppSettings> AppSettings => Set<AppSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
