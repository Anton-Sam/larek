using BuildingBlocks.Application.Exceptions;
using Catalog.Application.Common.Data;
using Catalog.Domain.ItemAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Items.Commands.SellItems;

public class SellItemsCommandHandler : IRequestHandler<SellItemsCommand, Item>
{
    private readonly IAppDbContext _dbContext;

    public SellItemsCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Item> Handle(SellItemsCommand request, CancellationToken cancellationToken)
    {
        var item = await _dbContext.Items.FirstOrDefaultAsync(
            i => i.Id == request.ItemId,
            cancellationToken);

        if (item is null)
            throw new NotFoundException("Item not found");

        item.Sell(request.Count);
        await _dbContext.SaveChangesAsync();

        return item;
    }
}
