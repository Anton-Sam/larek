using Catalog.Domain.CategoryAggregate;
using MediatR;

namespace Catalog.Application.Features.Categories.Queries.GetAllCategories;

public record GetAllCategoriesQuery : IRequest<IEnumerable<Category>>;
