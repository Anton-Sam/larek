using BuildingBlocks.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Data;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Order>
{
    private readonly IAppDbContext _dbContext;
    private readonly ICatalogService _catalogService;

    public CancelOrderCommandHandler(
        IAppDbContext dbContext,
        ICatalogService catalogService)
    {
        _dbContext = dbContext;
        _catalogService = catalogService;
    }

    public async Task<Order> Handle(
        CancelOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);

        if (order is null)
            throw new NotFoundException("Order not found");

        order.Cancel();

        var tasks = new List<Task>();
        foreach (var item in order.Items)
        {
            tasks.Add(_catalogService.ReleaseItemsAsync(item.ItemId, item.UnitCount));
        }
        await Task.WhenAll(tasks);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return order;
    }
}
