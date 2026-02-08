using RannaTask.DAL.Contexts;
using RannaTask.Entities.Common;
using RannaTask.Entities.Entities;
using Microsoft.Extensions.Logging;

namespace RannaTask.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context, ILogger logger)
        {
            try
            {
                // Admin kullanıcısı zaten var mı kontrol et
                if (context.Users.Any(u => u.Username == "admin"))
                {
                    logger.LogInformation("Seed data already exists. Skipping...");
                    return;
                }

                logger.LogInformation("Creating default users...");

                // 1. Admin kullanıcısı
                var adminUser = new User
                {
                    Username = "admin",
                    Email = "admin@rannatask.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    FirstName = "Admin",
                    LastName = "User",
                    Role = UserRole.Admin,
                    IsActive = true,
                    Created = DateTime.UtcNow
                };

                // 2. Test Manager (opsiyonel)
                var managerUser = new User
                {
                    Username = "manager",
                    Email = "manager@rannatask.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager123!"),
                    FirstName = "Manager",
                    LastName = "User",
                    Role = UserRole.Manager,
                    IsActive = true,
                    Created = DateTime.UtcNow
                };

                // 3. Test Customer (opsiyonel)
                var customerUser = new User
                {
                    Username = "customer",
                    Email = "customer@rannatask.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer123!"),
                    FirstName = "Test",
                    LastName = "Customer",
                    Role = UserRole.Customer,
                    IsActive = true,
                    Created = DateTime.UtcNow
                };

                context.Users.AddRange(adminUser, managerUser, customerUser);
                context.SaveChanges();

                logger.LogInformation("✅ Default users created successfully!");
                logger.LogInformation("   [ADMIN]    Username: admin     | Password: Admin123!");
                logger.LogInformation("   [MANAGER]  Username: manager   | Password: Manager123!");
                logger.LogInformation("   [CUSTOMER] Username: customer  | Password: Customer123!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
    }
}
