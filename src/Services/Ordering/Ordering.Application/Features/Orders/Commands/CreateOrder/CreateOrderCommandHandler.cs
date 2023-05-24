using BuildingBlocks.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Data;
using Ordering.Domain.OrderAggregate;

namespace Ordering.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly IAppDbContext _dbContext;

    public CreateOrderCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
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

        var order = Order.Create(request.BuyerId, addr, request.DeliveryType);

        await _dbContext.Orders.AddAsync(order, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return order;
    }
}
