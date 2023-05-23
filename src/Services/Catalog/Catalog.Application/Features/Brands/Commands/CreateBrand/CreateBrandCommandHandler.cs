using Catalog.Application.Common.Data;
using Catalog.Domain.BrandAggregate;
using MediatR;

namespace Catalog.Application.Features.Brands.Commands.CreateBrand;

public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Brand>
{
    private readonly IAppDbContext _dbContext;

    public CreateBrandCommandHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Brand> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = Brand.Create(request.Name);

        await _dbContext.Brands.AddAsync(brand, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return brand;
    }
}
