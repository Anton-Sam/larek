using MediatR;
using Ordering.Domain.BuyerAggregate;

namespace Ordering.Application.Features.Buyers.Queries.GetBuyerById;

public record GetBuyerByIdQuery(Guid BuyerId)
    : IRequest<Buyer>;
