using Catalog.Domain.ItemAggregate;
using MediatR;

namespace Catalog.Application.Features.Items.Commands.UpdateAvailableItemCount;

public record UpdateAvailableItemCountCommand(
    Guid ItemId,
    uint Count) : IRequest<Item>;
