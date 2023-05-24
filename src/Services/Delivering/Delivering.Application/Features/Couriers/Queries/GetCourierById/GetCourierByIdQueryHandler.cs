using BuildingBlocks.Application.Exceptions;
using Delivering.Application.Common.Data;
using Delivering.Domain.СourierAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Delivering.Application.Features.Couriers.Queries.GetCourierById;

public class GetCourierByIdQueryHandler
    : IRequestHandler<GetCourierByIdQuery, Courier>
{
    private readonly IAppDbContext _dbContext;

    public GetCourierByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Courier> Handle(
        GetCourierByIdQuery request,
        CancellationToken cancellationToken)
    {
        var courier = await _dbContext.Couriers
            .FirstOrDefaultAsync(
            c => c.Id == request.CourierId,
            cancellationToken);

        if (courier is null)
            throw new NotFoundException("Courier not found");

        return courier;
    }
}
