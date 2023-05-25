using Catalog.Domain.ItemAggregate;
using MediatR;

namespace Catalog.Application.Features.Items.Commands.AddItems;

public record AddItemsCommand(
    Guid ItemId,
    uint Count) : IRequest<Item>;
