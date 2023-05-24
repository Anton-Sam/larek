using Delivering.Application.Common.Data;
using Delivering.Domain.СourierAggregate;
using MediatR;

namespace Delivering.Application.Features.Couriers.Command.CreateCourier;

public class CreateCourierCommandHandler : IRequestHandler<CreateCourierCommand, Courier>
{
    private readonly IAppDbContext _dbContext;

    public CreateCourierCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Courier> Handle(
        CreateCourierCommand request,
        CancellationToken cancellationToken)
    {
        var courier = Courier.Create(request.Name);

        await _dbContext.Couriers.AddAsync(courier, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return courier;
    }
}
