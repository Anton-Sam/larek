using BuildingBlocks.Application.Exceptions;
using Catalog.Application.Common.Data;
using Catalog.Domain.ItemAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Items.Commands.ReleaseItems;

internal class ReleaseItemsCommandHandler
    : IRequestHandler<ReleaseItemsCommand, Item>
{
    private readonly IAppDbContext _dbContext;

    public ReleaseItemsCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Item> Handle(
        ReleaseItemsCommand request,
        CancellationToken cancellationToken)
    {
        var item = await _dbContext.Items.FirstOrDefaultAsync(
            i => i.Id == request.ItemId,
            cancellationToken);

        if (item is null)
            throw new NotFoundException("Item not found");

        item.Release(request.Count);
        await _dbContext.SaveChangesAsync();

        return item;
    }
}
