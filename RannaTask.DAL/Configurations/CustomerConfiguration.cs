using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RannaTask.Entities.Entities;

namespace RannaTask.Entities.Customers;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);

        var customer1 = new Customer()
        {
            Id = 1,
            FirstName="Ahmet",
            LastName="Şaşmaz",
            Email="ahmetsasmaz@gmail.com"
        };
        var customer2 = new Customer()
        {
            Id = 2,
            FirstName = "Mehmet",
            LastName = "Üzülmez",
            Email = "mehmetuzulmez@gmail.com"
        };
        builder.HasData(customer1, customer2);
    }
}


