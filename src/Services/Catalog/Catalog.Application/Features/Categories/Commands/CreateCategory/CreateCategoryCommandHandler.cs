using Catalog.Application.Common.Data;
using Catalog.Domain.CategoryAggregate;
using MediatR;

namespace Catalog.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly IAppDbContext _dbContext;

    public CreateCategoryCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = Category.Create(request.Name);

        await _dbContext.Categories.AddAsync(category, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return category;
    }
}
