using Delivering.Domain.СourierAggregate;
using MediatR;

namespace Delivering.Application.Features.Couriers.Queries.GetCourierById;

public record GetCourierByIdQuery(Guid CourierId)
    : IRequest<Courier>;
