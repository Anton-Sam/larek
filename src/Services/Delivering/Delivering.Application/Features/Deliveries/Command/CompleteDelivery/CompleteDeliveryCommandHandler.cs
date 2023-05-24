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

    public CompleteDeliveryCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
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
        await _dbContext.SaveChangesAsync(cancellationToken);

        return delivery;
    }
}
