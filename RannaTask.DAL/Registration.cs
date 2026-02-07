using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RannaTask.DAL.Contexts;
using RannaTask.DAL.Repositories;
using RannaTask.DAL.Repositories.Notifications;
using RannaTask.DAL.Repositories.Products;
using RannaTask.DAL.Repositories.SupportForms;
using RannaTask.DAL.Repositories.Users;
using RannaTask.DAL.UnitOfWorks;

namespace RannaTask.DAL;

public static class Registration
{
    public static IServiceCollection AddDALRegistiration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISupportFormRepository, SupportFormRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();

        services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
