using Catalog.Domain.BrandAggregate;
using Catalog.Domain.CategoryAggregate;
using Catalog.Domain.ItemAggregate;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Common.Data;

public interface IAppDbContext
{
    DbSet<Brand> Brands { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<Item> Items { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
