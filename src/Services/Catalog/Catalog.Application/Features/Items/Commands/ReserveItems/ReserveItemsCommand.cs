using Catalog.Domain.ItemAggregate;
using MediatR;

namespace Catalog.Application.Features.Items.Commands.ReserveItems;

public record ReserveItemsCommand(Guid ItemId, uint Count)
    : IRequest<Item>;
