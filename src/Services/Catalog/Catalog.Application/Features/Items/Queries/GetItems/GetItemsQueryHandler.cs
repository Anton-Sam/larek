using Catalog.Application.Common.Data;
using Catalog.Domain.ItemAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Items.Queries.GetItems;

public class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, IEnumerable<Item>>
{
    private readonly IAppDbContext _dbContext;

    public GetItemsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Item>> Handle(
        GetItemsQuery request,
        CancellationToken cancellationToken)
    {
        var items = await _dbContext.Items.ToListAsync(cancellationToken);

        return items ?? Enumerable.Empty<Item>();
    }
}
