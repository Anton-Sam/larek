using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.AddOrderItems;
using Ordering.Application.Features.Orders.Commands.CancelOrder;
using Ordering.Application.Features.Orders.Commands.CreateOrder;
using Ordering.Application.Features.Orders.Commands.PickupOrder;
using Ordering.Application.Features.Orders.Queries.GetOrderById;
using Ordering.Application.Features.Orders.Queries.GetOrders;
using Ordering.WebApi.Dtos.Requests.Orders;
using Ordering.WebApi.Dtos.Responses.Orders;

namespace Ordering.WebApi.Controllers;

[Route("api/ordering/orders")]
public class OrdersController : ApiController
{
    public OrdersController(ISender sender) : base(sender)
    { }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var order = await Sender.Send(new CreateOrderCommand(
            request.BuyerId,
            request.Street,
            request.City,
            request.State,
            request.Country,
            request.ZipCode,
            request.DeliveryType), cancellationToken);

        var items = order.Items.Select(i => new OrderItemResponse(
            i.ItemId,
            i.UnitPrice,
            i.UnitCount));
        var response = new OrderResponse(
            order.Id,
            order.BuyerId,
            order.Address.Street,
            order.Address.City,
            order.Address.State,
            order.Address.Country,
            order.Address.ZipCode,
            order.TotalCount,
            order.TotalPrice,
            order.OrderStatus,
            order.DeliveryType,
            items);

        return CreatedAtAction(
            nameof(GetOrderById),
            new { Id = order.Id },
            response);
    }

    [HttpPut("{id}/item")]
    public async Task<IActionResult> AddOrderItem(
        Guid id,
        [FromBody] OrderItemRequest request,
        CancellationToken cancellationToken)
    {
        var order = await Sender.Send(new AddOrderItemsCommand(
            id,
            request.ItemId,
            request.Price,
            request.Count), cancellationToken);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(
        Guid buyerId,
        CancellationToken cancellationToken)
    {
        var orders = await Sender.Send(
            new GetOrdersQuery(buyerId),
            cancellationToken);

        var response = orders.Select(ord =>
        {
            var items = ord.Items
            .Select(i => new OrderItemResponse(
                i.ItemId,
                i.UnitPrice,
                i.UnitCount));

            return new OrderResponse(
                ord.Id,
                ord.BuyerId,
                ord.Address.Street,
                ord.Address.City,
                ord.Address.State,
                ord.Address.Country,
                ord.Address.ZipCode,
                ord.TotalCount,
                ord.TotalPrice,
                ord.OrderStatus,
                ord.DeliveryType,
                items);
        });

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var order = await Sender.Send(
            new GetOrderByIdQuery(id),
            cancellationToken);

        var items = order.Items.Select(i => new OrderItemResponse(
            i.ItemId,
            i.UnitPrice,
            i.UnitCount));
        var response = new OrderResponse(
            order.Id,
            order.BuyerId,
            order.Address.Street,
            order.Address.City,
            order.Address.State,
            order.Address.Country,
            order.Address.ZipCode,
            order.TotalCount,
            order.TotalPrice,
            order.OrderStatus,
            order.DeliveryType,
            items);

        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> PickupOrder(
        [FromBody] PickupOrderRequest request,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new PickupOrderCommand(request.OrderId),
            cancellationToken);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> CancelOrder(
        [FromBody] CancelOrderRequest request,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new CancelOrderCommand(request.OrderId),
            cancellationToken);

        return Ok();
    }
}
