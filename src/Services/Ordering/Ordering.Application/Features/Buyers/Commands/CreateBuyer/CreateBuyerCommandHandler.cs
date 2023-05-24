using MediatR;
using Ordering.Application.Common.Data;
using Ordering.Domain.BuyerAggregate;

namespace Ordering.Application.Features.Buyers.Commands.CreateBuyer;

public class CreateBuyerCommandHandler : IRequestHandler<CreateBuyerCommand, Buyer>
{
    private readonly IAppDbContext _dbContext;

    public CreateBuyerCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Buyer> Handle(
        CreateBuyerCommand request,
        CancellationToken cancellationToken)
    {
        var buyer = Buyer.Create(request.Name);

        await _dbContext.Buyers.AddAsync(buyer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return buyer;
    }
}
