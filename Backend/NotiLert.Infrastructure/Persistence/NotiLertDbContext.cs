namespace NotiLert.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using NotiLert.Domain.Entities;

public class NotiLertDbContext : DbContext
{
    public NotiLertDbContext(DbContextOptions<NotiLertDbContext> options)
        : base(options)
    {
    }

    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(NotiLertDbContext).Assembly);
    }
}
