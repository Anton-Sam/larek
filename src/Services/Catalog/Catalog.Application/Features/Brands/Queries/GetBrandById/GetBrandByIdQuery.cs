using Catalog.Domain.BrandAggregate;
using MediatR;

namespace Catalog.Application.Features.Brands.Queries.GetBrandById;

public record GetBrandByIdQuery(Guid BrandId)
    : IRequest<Brand>;
