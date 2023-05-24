using Catalog.Domain.ItemAggregate;
using MediatR;

namespace Catalog.Application.Features.Items.Queries.GetItemById;

public record GetItemByIdQuery(Guid ItemId)
    : IRequest<Item>;
