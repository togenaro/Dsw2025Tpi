using Dsw2025Tpi.Application.Services;
using Dsw2025Tpi.Data;
using Dsw2025Tpi.Data.Repositories;
using Dsw2025Tpi.Domain.Entities;
using Dsw2025Tpi.Domain.Interfaces;
using Dsw2025Tpi.Data.Helpers;

using Microsoft.EntityFrameworkCore;


namespace Dsw2025Tpi.Api.Configurations;

public static class DomainServicesConfigurationExtension
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Dsw2025TpiContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Dsw2025TpiEntities"));
            options.UseSeeding((c, t) =>
            {
                ((Dsw2025TpiContext)c).Seedwork<Product>("Sources\\products.json");
                ((Dsw2025TpiContext)c).Seedwork<Customer>("Sources\\customers.json");
                ((Dsw2025TpiContext)c).Seedwork<Order>("Sources\\orders.json");
                ((Dsw2025TpiContext)c).Seedwork<OrderItem>("Sources\\orderItems.json");
            });
        });

        //services.AddScoped<IRepository, InMemoryRepository>();
        services.AddScoped<IRepository, EfRepository>();
        services.AddTransient<ProductsManagementService>();
        services.AddTransient<OrderManagementService>();

        return services;
    }
}

