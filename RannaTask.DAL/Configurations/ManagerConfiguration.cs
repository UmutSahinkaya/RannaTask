using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RannaTask.Entities.Entities;

namespace RannaTask.DAL.Configurations
{
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.HasKey(x => x.Id);

            var manager = new Manager()
            {
                Id = 1,
                FirstName = "Manager",
                LastName = "",
                Email = "manager@gmail.com"
            };
            
            builder.HasData(manager);
        }
    }

}
