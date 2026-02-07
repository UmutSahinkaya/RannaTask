using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RannaTask.Entities.Entities;

namespace RannaTask.DAL.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Message).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.Type).IsRequired();
        builder.Property(x => x.IsRead).IsRequired();
        builder.Property(x => x.UserId).IsRequired(false); // Nullable

        builder.HasOne(x => x.User)
               .WithMany(u => u.Notifications)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.SetNull)
               .IsRequired(false); // Nullable relationship
    }
}
