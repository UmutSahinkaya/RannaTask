using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RannaTask.Entities.Entities;

namespace RannaTask.Entities.Customers;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(250);

        var product1 = new Product()
        {
            Id = 1,
            Name = "AbcYazılım",
            Code="ABC123",
            Price=100000
        };
        builder.HasData(product1);
    }
}

