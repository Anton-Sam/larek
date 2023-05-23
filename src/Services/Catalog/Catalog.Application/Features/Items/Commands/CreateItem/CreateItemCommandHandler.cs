using Catalog.Application.Common.Data;
using Catalog.Domain.ItemAggregate;
using MediatR;

namespace Catalog.Application.Features.Items.Commands.CreateItem;

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Item>
{
    private readonly IAppDbContext _dbContext;

    public CreateItemCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Item> Handle(CreateItemCommand request, CancellationToken cancellationToken)
    {
        var item = Item.Create(
            request.BrandId,
            request.CategoryId,
            request.Price,
            request.Name,
            request.Description,
            request.Count);

        await _dbContext.Items
            .AddAsync(item, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return item;
    }
}
