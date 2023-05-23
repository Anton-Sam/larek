using Catalog.Domain.ItemAggregate;
using MediatR;

namespace Catalog.Application.Features.Items.Commands.CreateItem;

public record CreateItemCommand(
    Guid BrandId,
    Guid CategoryId,
    decimal Price,
    string Name,
    string Description,
    uint Count) : IRequest<Item>;
