using BuildingBlocks.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Data;
using Ordering.Domain.BuyerAggregate;

namespace Ordering.Application.Features.Buyers.Queries.GetBuyerById;

public class GetBuyerByIdQueryHandler : IRequestHandler<GetBuyerByIdQuery, Buyer>
{
    private readonly IAppDbContext _dbContext;

    public GetBuyerByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Buyer> Handle(
        GetBuyerByIdQuery request,
        CancellationToken cancellationToken)
    {
        var buyer = await _dbContext.Buyers
            .FirstOrDefaultAsync(b => b.Id == request.BuyerId);

        if (buyer is null)
            throw new NotFoundException("Buyer not found");

        return buyer;
    }
}
