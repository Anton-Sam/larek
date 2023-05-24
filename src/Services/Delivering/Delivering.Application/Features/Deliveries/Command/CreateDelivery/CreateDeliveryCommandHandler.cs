using Delivering.Application.Common.Data;
using Delivering.Domain.DeliveryAggregate;
using MediatR;

namespace Delivering.Application.Features.Deliveries.Command.CreateDelivery;

public class CreateDeliveryCommandHandler
    : IRequestHandler<CreateDeliveryCommand, Delivery>
{
    private readonly IAppDbContext _dbContext;

    public CreateDeliveryCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Delivery> Handle(
        CreateDeliveryCommand request,
        CancellationToken cancellationToken)
    {
        var delivery = Delivery.Create(request.OrderId, request.DeliveryDate);

        await _dbContext.Deliveries.AddAsync(delivery, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return delivery;
    }
}
