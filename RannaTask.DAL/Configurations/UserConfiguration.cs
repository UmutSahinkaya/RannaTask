using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RannaTask.Entities.Common;
using RannaTask.Entities.Entities;

namespace RannaTask.Entities.Customers;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Username).IsRequired().HasMaxLength(150);
        builder.Property(x => x.PasswordHash).IsRequired().HasMaxLength(150);
        
        //var user1 = new User()
        //{
        //    Id = 1,
        //    Username = "ahmetsasmaz",
        //    RoleId=2,
        //    PasswordHash=,
        //    CustomerId=1,
        //};
        //var user2 = new User()
        //{
        //    Id = 2,
        //    Username = "Manager",
        //    PasswordHash = "asdasd",
        //    RoleId =1,
        //    ManagerId=1
        //};
        
        //builder.HasData(user1,user2);
    }
}

