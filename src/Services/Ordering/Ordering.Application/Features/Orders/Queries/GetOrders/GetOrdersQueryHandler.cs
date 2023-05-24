using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Data;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<Order>>
{
    private readonly IAppDbContext _dbContext;

    public GetOrdersQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Order>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<Order> query = _dbContext.Orders;

        if (request.BuyerId != Guid.Empty)
            query = query.Where(o => o.BuyerId == request.BuyerId);

        var orders = await query.ToListAsync(cancellationToken);

        return orders ?? Enumerable.Empty<Order>();
    }
}
