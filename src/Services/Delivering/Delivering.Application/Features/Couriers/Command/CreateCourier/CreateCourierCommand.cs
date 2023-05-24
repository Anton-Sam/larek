using Delivering.Domain.СourierAggregate;
using MediatR;

namespace Delivering.Application.Features.Couriers.Command.CreateCourier;

public record CreateCourierCommand(string Name)
    : IRequest<Courier>;
