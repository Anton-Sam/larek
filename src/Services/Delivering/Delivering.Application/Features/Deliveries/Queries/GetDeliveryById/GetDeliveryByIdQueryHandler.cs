using BuildingBlocks.Application.Exceptions;
using Delivering.Application.Common.Data;
using Delivering.Domain.DeliveryAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Delivering.Application.Features.Deliveries.Queries.GetDeliveryById;

public class GetDeliveryByIdQueryHandler
    : IRequestHandler<GetDeliveryByIdQuery, Delivery>
{
    private readonly IAppDbContext _dbContext;

    public GetDeliveryByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Delivery> Handle(
        GetDeliveryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var delivery = await _dbContext.Deliveries
            .FirstOrDefaultAsync(
            c => c.Id == request.DeliveryId,
            cancellationToken);

        if (delivery is null)
            throw new NotFoundException("Courier not found");

        return delivery;
    }
}
