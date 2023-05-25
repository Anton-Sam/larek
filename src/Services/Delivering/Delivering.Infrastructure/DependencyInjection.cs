using Delivering.Application.Common.Data;
using Delivering.Infrastructure.Data;
using Delivering.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Delivering.Infrastructure;

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

        services.AddScoped<IOrderService, OrderService>();
        services.AddHttpClient<IOrderService, OrderService>(
            client => client.BaseAddress = new Uri(configuration["OrderUrl"] ?? string.Empty));

        return services;
    }
}
