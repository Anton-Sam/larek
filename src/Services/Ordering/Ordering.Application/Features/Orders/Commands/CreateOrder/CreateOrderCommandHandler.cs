using BuildingBlocks.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Data;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly IAppDbContext _dbContext;
    private readonly IDeliveryService _deliveryService;

    public CreateOrderCommandHandler(
        IAppDbContext dbContext,
        IDeliveryService deliveryService)
    {
        _dbContext = dbContext;
        _deliveryService = deliveryService;
    }

    public async Task<Order> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var buyer = await _dbContext.Buyers
            .FirstOrDefaultAsync(b => b.Id == request.BuyerId);

        if (buyer is null)
            throw new NotFoundException("Buyer not found");

        var addr = Address.Create(
            request.Street,
            request.City,
            request.State,
            request.Country,
            request.ZipCode);

        Order order = request.DeliveryType == DeliveryType.Delivery
            ? Order.CreateDeliveryOrder(request.BuyerId, addr, request.DeliveryDate)
            : Order.CreatePickupOrder(request.BuyerId, addr);

        if (order.DeliveryType == DeliveryType.Delivery)
            await _deliveryService.CreateDeliveryAsync(order);

        await _dbContext.Orders.AddAsync(order, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return order;
    }
}
