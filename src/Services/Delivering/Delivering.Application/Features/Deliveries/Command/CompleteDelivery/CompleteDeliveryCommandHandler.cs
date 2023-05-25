using BuildingBlocks.Application.Exceptions;
using Delivering.Application.Common.Data;
using Delivering.Domain.DeliveryAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Delivering.Application.Features.Deliveries.Command.CompleteDelivery;

public class CompleteDeliveryCommandHandler
    : IRequestHandler<CompleteDeliveryCommand, Delivery>
{
    private readonly IAppDbContext _dbContext;
    private readonly IOrderService _orderService;

    public CompleteDeliveryCommandHandler(
        IAppDbContext dbContext,
        IOrderService orderService)
    {
        _dbContext = dbContext;
        _orderService = orderService;
    }

    public async Task<Delivery> Handle(
        CompleteDeliveryCommand request,
        CancellationToken cancellationToken)
    {
        var delivery = await _dbContext.Deliveries
            .FirstOrDefaultAsync(
            d => d.Id == request.DeliveryId,
            cancellationToken);

        if (delivery is null)
            throw new NotFoundException("Delivery not found");

        delivery.Complete();

        await _orderService.CompleteOrderAsync(delivery.OrderId, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return delivery;
    }
}
