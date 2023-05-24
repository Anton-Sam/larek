using BuildingBlocks.Application.Exceptions;
using Delivering.Application.Common.Data;
using Delivering.Domain.DeliveryAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Delivering.Application.Features.Deliveries.Command.StartProcessDelivery;

public class StartProcessDeliveryCommandHandler
    : IRequestHandler<StartProcessDeliveryCommand, Delivery>
{
    private readonly IAppDbContext _dbContext;

    public StartProcessDeliveryCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Delivery> Handle(
        StartProcessDeliveryCommand request,
        CancellationToken cancellationToken)
    {
        var courier = await _dbContext.Couriers
            .FirstOrDefaultAsync(
            c => c.Id == request.CourierId,
            cancellationToken);

        if (courier is null)
            throw new NotFoundException("Courier not found");

        var delivery = await _dbContext.Deliveries
            .FirstOrDefaultAsync(
            d => d.Id == request.DeliveryId,
            cancellationToken);

        if (delivery is null)
            throw new NotFoundException("Delivery not found");

        delivery.StartProcess(courier.Id);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return delivery;
    }
}
