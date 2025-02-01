using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RannaTask.Entities.Common;
using RannaTask.Entities.Entities;

namespace RannaTask.Entities.Customers;

public class SupportFormConfiguration : IEntityTypeConfiguration<SupportForm>
{
    public void Configure(EntityTypeBuilder<SupportForm> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Subject).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Message).IsRequired().HasMaxLength(250);

        var form1 = new SupportForm()
        {
            Id = 1,
            Subject="Test",
            Message="TEst Deneme",
            CustomerId=1,
            Status=SupportFormStatus.Pending
        };
        builder.HasData(form1);
    }
}

