using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RannaTask.Business.Helpers;
using RannaTask.Business.Notifications;
using RannaTask.Business.Products;
using RannaTask.Business.SupportForms;
using RannaTask.Business.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RannaTask.Business.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IProductService, ProductManager>();
        services.AddScoped<IUserService, UserManager>();
        services.AddScoped<ISupportFormService, SupportFormManager>();
        services.AddScoped<INotificationService, NotificationManager>();
        services.AddSingleton<ITokenService, TokenManager>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
