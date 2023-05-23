using Catalog.Application.Common.Data;
using Catalog.Domain.CategoryAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Categories.Queries.GetAllCategories;

public class GetAllCategoriesQueryHandler
    : IRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllCategoriesQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Category>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await _dbContext.Categories
            .ToListAsync(cancellationToken);

        return categories ?? Enumerable.Empty<Category>();
    }
}
