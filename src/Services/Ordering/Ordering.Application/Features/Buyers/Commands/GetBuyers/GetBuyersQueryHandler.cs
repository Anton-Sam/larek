using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Data;
using Ordering.Domain.BuyerAggregate;

namespace Ordering.Application.Features.Buyers.Commands.GetBuyers;

public class GetBuyersQueryHandler : IRequestHandler<GetBuyersQuery, IEnumerable<Buyer>>
{
    private readonly IAppDbContext _dbContext;

    public GetBuyersQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Buyer>> Handle(
        GetBuyersQuery request,
        CancellationToken cancellationToken)
    {
        var buyers = await _dbContext.Buyers
            .ToListAsync(cancellationToken);

        return buyers ?? Enumerable.Empty<Buyer>();
    }
}
