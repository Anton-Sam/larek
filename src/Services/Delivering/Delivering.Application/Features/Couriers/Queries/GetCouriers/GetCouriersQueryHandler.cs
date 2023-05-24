using Delivering.Application.Common.Data;
using Delivering.Domain.СourierAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Delivering.Application.Features.Couriers.Queries.GetCouriers;

public class GetCouriersQueryHandler
    : IRequestHandler<GetCouriersQuery, IEnumerable<Courier>>
{
    private readonly IAppDbContext _dbContext;

    public GetCouriersQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Courier>> Handle(
        GetCouriersQuery request,
        CancellationToken cancellationToken)
    {
        var couriers = await _dbContext.Couriers
            .ToListAsync(cancellationToken);

        return couriers ?? Enumerable.Empty<Courier>();
    }
}
