using Catalog.Domain.BrandAggregate;
using MediatR;

namespace Catalog.Application.Features.Brands.Commands.CreateBrand;

public record CreateBrandCommand(string Name) : IRequest<Brand>;
