using Catalog.Domain.ItemAggregate;
using MediatR;

namespace Catalog.Application.Features.Items.Commands.ReleaseItems;

public record ReleaseItemsCommand(Guid ItemId, uint Count)
    : IRequest<Item>;
