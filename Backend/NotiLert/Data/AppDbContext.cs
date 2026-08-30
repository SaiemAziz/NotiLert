using Microsoft.EntityFrameworkCore;
using NotiLert.Models;
namespace NotiLert.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<EmailRecipient> EmailRecipients => Set<EmailRecipient>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmailRecipient>()
                .Property(e => e.IsActive)
                .HasDefaultValue(true);
        }
    }
}
