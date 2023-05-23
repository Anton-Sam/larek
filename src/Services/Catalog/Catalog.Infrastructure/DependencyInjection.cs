using Catalog.Application.Common.Data;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure;

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

        return services;
    }
}
