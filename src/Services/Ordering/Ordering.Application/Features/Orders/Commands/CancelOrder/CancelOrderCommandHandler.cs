using BuildingBlocks.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Data;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Order>
{
    private readonly IAppDbContext _dbContext;

    public CancelOrderCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> Handle(
        CancelOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order is null)
            throw new NotFoundException("Order not found");

        //HTTP service

        order.Fill();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return order;
    }
}
