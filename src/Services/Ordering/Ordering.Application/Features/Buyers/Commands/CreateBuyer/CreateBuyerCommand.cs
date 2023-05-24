using MediatR;
using Ordering.Domain.BuyerAggregate;

namespace Ordering.Application.Features.Buyers.Commands.CreateBuyer;

public record CreateBuyerCommand(string Name)
    : IRequest<Buyer>;
