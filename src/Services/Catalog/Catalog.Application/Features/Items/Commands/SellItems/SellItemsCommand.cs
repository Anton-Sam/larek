using Catalog.Domain.ItemAggregate;
using MediatR;

namespace Catalog.Application.Features.Items.Commands.SellItems;

public record SellItemsCommand(Guid ItemId, uint Count)
    : IRequest<Item>;
