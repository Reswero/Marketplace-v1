using Marketplace.Common.Authorization.Attributes;
using Marketplace.Common.Authorization.Models;
using Marketplace.Common.Responses;
using Marketplace.Orders.Application.Common.ViewModels;
using Marketplace.Orders.Application.Orders.Commands.CancelOrder;
using Marketplace.Orders.Application.Orders.Commands.CreateOrder;
using Marketplace.Orders.Application.Orders.Commands.PackOrderProduct;
using Marketplace.Orders.Application.Orders.Queries.GetCustomerOrder;
using Marketplace.Orders.Application.Orders.Queries.GetCustomerOrders;
using Marketplace.Orders.Application.Orders.Queries.GetOrderPaymentLink;
using Marketplace.Orders.Application.Orders.Queries.GetSellerOrders;
using Marketplace.Orders.Application.Orders.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Orders.Api.Controllers;

/// <summary>
/// Взаимодействие с заказами
/// </summary>
[ApiController]
[Route("v1")]
public class OrdersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Создание заказа
    /// </summary>
    /// <param name="cmd"></param>
    [HttpPost("/internal/v1/orders")]
    [ProducesResponseType(typeof(CreateObjectResultVM), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(CreateOrderCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// Получение заказов покупателя
    /// </summary>
    /// <param name="query"></param>
    [HttpGet("customer/orders")]
    [AccountTypeAuthorize(AccountType.Customer)]
    [ProducesResponseType(typeof(List<OrderShortInfoVM>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCustomerOrders([FromQuery] GetCustomerOrdersQuery query)
    {
        var orders = await _mediator.Send(query);
        return Ok(orders);
    }

    /// <summary>
    /// Получение заказа покупателя
    /// </summary>
    /// <param name="orderId">Идентификатор заказа</param>
    [HttpGet("customer/orders/{orderId:long}")]
    [AccountTypeAuthorize(AccountType.Customer)]
    [ProducesResponseType(typeof(OrderVM), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCustomerOrder(long orderId)
    {
        var order = await _mediator.Send(new GetCustomerOrderQuery(orderId));
        return Ok(order);
    }

    /// <summary>
    /// Получение ссылки на оплату заказа
    /// </summary>
    /// <param name="orderId">Идентификатор заказа</param>
    [HttpGet("customer/orders/{orderId:long}/payment")]
    [AccountTypeAuthorize(AccountType.Customer)]
    [ProducesResponseType(typeof(PaymentVM), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrderPaymentLink(long orderId)
    {
        var payment = await _mediator.Send(new GetOrderPaymentLinkQuery(orderId));
        return Ok(payment);
    }

    /// <summary>
    /// Отмена заказа покупателя
    /// </summary>
    /// <param name="orderId">Идентификатор заказа</param>
    /// <param name="productIds">Идентификаторы товаров</param>
    [HttpDelete("customer/orders/{orderId:long}")]
    [AccountTypeAuthorize(AccountType.Customer)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CancelOrder(long orderId, [FromQuery] int[]? productIds)
    {
        await _mediator.Send(new CancelOrderCommand(orderId, productIds));
        return Ok();
    }

    /// <summary>
    /// Получение заказов продавца
    /// </summary>
    /// <param name="query"></param>
    [HttpGet("seller/orders")]
    [AccountTypeAuthorize(AccountType.Seller)]
    [ProducesResponseType(typeof(List<SellerOrderVM>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSellerOrders([FromQuery] GetSellerOrdersQuery query)
    {
        var orders = await _mediator.Send(query);
        return Ok(orders);
    }

    /// <summary>
    /// Установление товару статус "Упакован"
    /// </summary>
    /// <param name="orderProductId">Идентификатор товара из заказа</param>
    [HttpPost("seller/orders/{orderProductId:int}/pack")]
    [AccountTypeAuthorize(AccountType.Seller)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PackOrder(int orderProductId)
    {
        await _mediator.Send(new PackOrderProductCommand(orderProductId));
        return Ok();
    }
}
