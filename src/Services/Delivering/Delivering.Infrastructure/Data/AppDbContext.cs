using Delivering.Application.Common.Data;
using Delivering.Domain.DeliveryAggregate;
using Delivering.Domain.СourierAggregate;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Delivering.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Courier> Couriers { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
