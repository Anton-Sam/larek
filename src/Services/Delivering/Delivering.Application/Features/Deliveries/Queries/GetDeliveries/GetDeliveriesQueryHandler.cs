using Delivering.Application.Common.Data;
using Delivering.Domain.DeliveryAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Delivering.Application.Features.Deliveries.Queries.GetDeliveries;

public class GetDeliveriesQueryHandler
    : IRequestHandler<GetDeliveriesQuery, IEnumerable<Delivery>>
{
    private readonly IAppDbContext _dbContext;

    public GetDeliveriesQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Delivery>> Handle(
        GetDeliveriesQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<Delivery> query = _dbContext.Deliveries;

        if (request.CourierId != Guid.Empty)
            query = query.Where(o => o.CourierId == request.CourierId);

        var orders = await query.ToListAsync(cancellationToken);

        return orders ?? Enumerable.Empty<Delivery>();
    }
}
