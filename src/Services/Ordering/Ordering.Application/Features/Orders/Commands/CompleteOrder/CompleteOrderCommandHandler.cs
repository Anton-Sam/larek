using BuildingBlocks.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Data;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Commands.CompleteOrder;

public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommand, Order>
{
    private readonly IAppDbContext _dbContext;

    public CompleteOrderCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> Handle(
        CompleteOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId);

        if (order is null)
            throw new NotFoundException("Order not found");

        //HTTP service

        order.Cancel();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return order;
    }
}
