namespace NotiLert.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotiLert.Domain.Entities;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Recipient)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(n => n.Subject)
            .HasMaxLength(500);

        builder.Property(n => n.Body)
            .IsRequired();

        builder.Property(n => n.Channel)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(n => n.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(n => n.CreatedAtUtc)
            .IsRequired();

        builder.Property(n => n.SentAtUtc);

        builder.Property(n => n.ErrorMessage)
            .HasMaxLength(2000);

        builder.HasIndex(n => n.Status);
        builder.HasIndex(n => n.CreatedAtUtc);
    }
}
