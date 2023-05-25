using BuildingBlocks.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Data;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Commands.AddOrderItems;

public class AddOrderItemsCommandHandler
    : IRequestHandler<AddOrderItemsCommand, Order>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICatalogService _catalogService;

    public AddOrderItemsCommandHandler(
        IAppDbContext dbContext,
        ICatalogService catalogService)
    {
        _dbContext = dbContext;
        _catalogService = catalogService;
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

        var catalogItem = await _catalogService.GetItemAsync(
            request.ItemId,
            cancellationToken);

        if (catalogItem is null)
            throw new NotFoundException("Item not found");

        order.AddOrderItem(request.ItemId, catalogItem.Price, request.Count);

        await _catalogService.ReserveItemsAsync(
            catalogItem.Id,
            request.Count,
            cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return order;
    }
}
