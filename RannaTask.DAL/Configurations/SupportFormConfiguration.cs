using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RannaTask.Entities.Common;
using RannaTask.Entities.Entities;

namespace RannaTask.DAL.Configurations;

public class SupportFormConfiguration : IEntityTypeConfiguration<SupportForm>
{
    public void Configure(EntityTypeBuilder<SupportForm> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Subject).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Message).IsRequired().HasMaxLength(500);

        builder.HasOne(x => x.User)
               .WithMany(u => u.SupportForms)
               .HasForeignKey(x => x.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

