using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.AddOrderItems;
using Ordering.Application.Features.Orders.Commands.CancelOrder;
using Ordering.Application.Features.Orders.Commands.CompleteOrder;
using Ordering.Application.Features.Orders.Commands.CreateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrderById;
using Ordering.Application.Features.Orders.Queries.GetOrders;
using Ordering.Domain.OrderAggregate;
using Ordering.WebApi.Dtos.Requests.Orders;
using Ordering.WebApi.Dtos.Responses.Orders;

namespace Ordering.WebApi.Controllers;

[Route("api/ordering/orders")]
[Produces("application/json")]
public class OrdersController : ApiController
{
    public OrdersController(ISender sender) : base(sender)
    { }

    /// <summary>
    /// Create new empty order.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>New created order</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /orders
    ///     {
    ///         "buyerId": "5f144943-87a0-4519-994a-ff1137b09e28",
    ///         "street": "Lenina",
    ///         "city": "Minsk",
    ///         "state": "Minsk",
    ///         "country": "Belarus",
    ///         "zipCode": "220011",
    ///         "deliveryType": "Delivery",
    ///         "deliveryDate": "2023-05-25T10:41:26.854Z"
    ///     }
    /// </remarks>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="400">Invalid params</response>
    /// <response code="404">Buyer not found</response>
    [HttpPost]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse(request.DeliveryType, true, out DeliveryType deliveryType))
        {
            return BadRequest("Invalid delivery type");
        }

        var order = await Sender.Send(new CreateOrderCommand(
            request.BuyerId,
            request.Street,
            request.City,
            request.State,
            request.Country,
            request.ZipCode,
            deliveryType,
            request.DeliveryDate), cancellationToken);

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
            order.Status,
            order.DeliveryType,
            order.DeliveryDate,
            items);

        return CreatedAtAction(
            nameof(GetOrderById),
            new { Id = order.Id },
            response);
    }

    /// <summary>
    /// Add items to order.
    /// </summary>
    /// <remarks>
    /// Reserve items in Catalog service.
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Updated order</returns>
    /// <response code="200">Returns updated order</response>
    [HttpPost("{id}/item")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddOrderItem(
        Guid id,
        [FromBody] OrderItemRequest request,
        CancellationToken cancellationToken)
    {
        var order = await Sender.Send(new AddOrderItemsCommand(
            id,
            request.ItemId,
            request.Count), cancellationToken);

        return Ok(order);
    }

    /// <summary>
    /// Get either all or those belonging to a particular seller buyer orders 
    /// </summary>
    /// <param name="buyerId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of orders</returns>
    /// <response code="200">Returns list of orders</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
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
                ord.Status,
                ord.DeliveryType,
                ord.DeliveryDate,
                items);
        });

        return Ok(response);
    }

    /// <summary>
    /// Get order by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Returns order with id</response>
    /// <response code="404">If the order not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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
            order.Status,
            order.DeliveryType,
            order.DeliveryDate,
            items);

        return Ok(response);
    }

    /// <summary>
    /// Complete the order.
    /// </summary>
    /// <remarks>
    /// Used by the Delivery service after completing delivery or when the buyer pickup order.
    /// Sell items from Catalog service.
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Successful complete</response>
    /// <response code="400">If invalid order status</response>
    [HttpPatch("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PickupOrder(
        Guid id,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new CompleteOrderCommand(id),
            cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Cancel the order.
    /// </summary>
    /// <remarks>
    /// Used by the Delivery service after canceling delivery.
    /// Release reserved item from Catalog service.
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="204">Successful cancel</response>
    /// <response code="400">If invalid order status</response>
    [HttpPatch("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CancelOrder(
        Guid id,
        CancellationToken cancellationToken)
    {
        var _ = await Sender.Send(
            new CancelOrderCommand(id),
            cancellationToken);

        return NoContent();
    }
}
