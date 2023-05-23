using Catalog.Application.Common.Data;
using Catalog.Domain.BrandAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Brands.Queries.GetAllBrands;

public class GetAllBrandsQueryHandler
    : IRequestHandler<GetAllBrandsQuery, IEnumerable<Brand>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllBrandsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Brand>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = await _dbContext.Brands
            .ToListAsync(cancellationToken);

        return brands ?? Enumerable.Empty<Brand>();
    }
}
