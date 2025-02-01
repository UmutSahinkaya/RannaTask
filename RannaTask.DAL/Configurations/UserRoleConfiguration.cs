using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RannaTask.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.DAL.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Type).IsRequired();

            var userRole = new UserRole()
            {
                Id = 1,
                Name = "Manager",
                Type = 1
            };
            var userRole2 = new UserRole()
            {
                Id = 2,
                Name = "Customer",
                Type = 2
            };
            builder.HasData(userRole, userRole2);

        }
    }
}
