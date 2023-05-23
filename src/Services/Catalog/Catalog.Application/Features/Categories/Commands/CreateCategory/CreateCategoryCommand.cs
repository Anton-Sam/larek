using Catalog.Domain.CategoryAggregate;
using MediatR;

namespace Catalog.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Name) : IRequest<Category>;
