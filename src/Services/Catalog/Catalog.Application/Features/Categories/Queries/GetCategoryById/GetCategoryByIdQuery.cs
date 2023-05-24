using Catalog.Domain.CategoryAggregate;
using MediatR;

namespace Catalog.Application.Features.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid CategoryId)
    : IRequest<Category>;
