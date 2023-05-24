using Delivering.Domain.СourierAggregate;
using MediatR;

namespace Delivering.Application.Features.Couriers.Queries.GetCouriers;

public record GetCouriersQuery
    : IRequest<IEnumerable<Courier>>;

