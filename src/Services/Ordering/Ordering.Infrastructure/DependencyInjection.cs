using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Common.Data;
using Ordering.Application.Common.Interfaces;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Services;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(
            opt => opt.UseNpgsql(configuration.GetConnectionString("Npgsql")));
        services.AddScoped<IAppDbContext>(
            provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<ICatalogService, CatalogService>();
        services.AddHttpClient<ICatalogService, CatalogService>(
            client => client.BaseAddress = new Uri(configuration["CatalogUrl"] ?? string.Empty));

        services.AddScoped<IDeliveryService, DeliveryService>();
        services.AddHttpClient<IDeliveryService,DeliveryService>(
            client => client.BaseAddress = new Uri(configuration["DeliveryUrl"] ?? string.Empty));

        return services;
    }
}
