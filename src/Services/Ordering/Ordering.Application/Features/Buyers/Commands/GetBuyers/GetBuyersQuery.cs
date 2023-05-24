using MediatR;
using Ordering.Domain.BuyerAggregate;

namespace Ordering.Application.Features.Buyers.Commands.GetBuyers;

public record GetBuyersQuery() : IRequest<IEnumerable<Buyer>>;
