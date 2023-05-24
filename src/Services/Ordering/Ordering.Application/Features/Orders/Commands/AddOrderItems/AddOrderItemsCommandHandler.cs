using BuildingBlocks.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Data;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Commands.AddOrderItems;

public class AddOrderItemsCommandHandler
    : IRequestHandler<AddOrderItemsCommand, Order>
{
    private readonly IAppDbContext _dbContext;

    public AddOrderItemsCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> Handle(
        AddOrderItemsCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders
            .FirstOrDefaultAsync(
            o => o.Id == request.OrderId,
            cancellationToken);

        if (order is null)
            throw new NotFoundException("Order not found");

        //HTTP service

        order.AddOrderItem(request.ItemId, request.Price, request.Count);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return order;
    }
}
