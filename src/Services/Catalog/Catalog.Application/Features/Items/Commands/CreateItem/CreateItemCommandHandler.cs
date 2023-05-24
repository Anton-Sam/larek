using BuildingBlocks.Application.Exceptions;
using Catalog.Application.Common.Data;
using Catalog.Domain.ItemAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var brand = await _dbContext.Brands
            .FirstOrDefaultAsync(
            b => b.Id == request.BrandId,
            cancellationToken);

        if (brand is null)
        {
            throw new NotFoundException("Brand not found");
        }

        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(
            c => c.Id == request.CategoryId,
            cancellationToken);

        if (category is null)
        {
            throw new NotFoundException("Category not found");
        }

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
