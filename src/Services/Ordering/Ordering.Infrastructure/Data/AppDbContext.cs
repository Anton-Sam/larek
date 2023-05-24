using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Data;
using Ordering.Domain.BuyerAggregate;
using Ordering.Domain.OrderAggregate;
using System.Reflection;

namespace Ordering.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Buyer> Buyers { get; set; }

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
