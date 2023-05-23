using Catalog.Domain.BrandAggregate;
using MediatR;

namespace Catalog.Application.Features.Brands.Queries.GetAllBrands;

public record GetAllBrandsQuery : IRequest<IEnumerable<Brand>>;
