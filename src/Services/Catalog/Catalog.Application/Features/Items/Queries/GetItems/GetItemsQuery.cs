using Catalog.Domain.ItemAggregate;
using MediatR;

namespace Catalog.Application.Features.Items.Queries.GetItems;

public record GetItemsQuery() : IRequest<IEnumerable<Item>>;
