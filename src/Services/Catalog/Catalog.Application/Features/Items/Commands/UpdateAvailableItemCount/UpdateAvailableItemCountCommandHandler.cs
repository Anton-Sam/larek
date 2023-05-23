using BuildingBlocks.Application.Exceptions;
using Catalog.Application.Common.Data;
using Catalog.Domain.ItemAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Application.Features.Items.Commands.UpdateAvailableItemCount;

public class UpdateAvailableItemCountCommandHandler
    : IRequestHandler<UpdateAvailableItemCountCommand, Item>
{
    private readonly IAppDbContext _dbContext;

    public UpdateAvailableItemCountCommandHandler(
        IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Item> Handle(
        UpdateAvailableItemCountCommand request,
        CancellationToken cancellationToken)
    {
        var item = await _dbContext.Items.FirstOrDefaultAsync(
            i => i.Id == request.ItemId,
            cancellationToken);

        if (item is null)
            throw new NotFoundException("Item not found");

        item.AddItems(request.Count);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return item;
    }
}
