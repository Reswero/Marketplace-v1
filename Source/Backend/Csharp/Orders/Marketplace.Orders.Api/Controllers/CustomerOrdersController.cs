using Marketplace.Common.Authorization.Attributes;
using Marketplace.Common.Authorization.Models;
using Marketplace.Common.Responses;
using Marketplace.Orders.Application.Orders.Commands.CancelOrder;
using Marketplace.Orders.Application.Orders.Queries.GetCustomerOrder;
using Marketplace.Orders.Application.Orders.Queries.GetCustomerOrders;
using Marketplace.Orders.Application.Orders.Queries.GetOrderPaymentLink;
using Marketplace.Orders.Application.Orders.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Orders.Api.Controllers;

/// <summary>
/// Взаимодействие с заказами покупателей
/// </summary>
[ApiController]
[Route("v1/customer")]
public class CustomerOrdersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Получение заказов покупателя
    /// </summary>
    /// <param name="query"></param>
    [HttpGet("orders")]
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
    [HttpGet("orders/{orderId:long}")]
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
    [HttpGet("orders/{orderId:long}/payment")]
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
    [HttpDelete("orders/{orderId:long}")]
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
}
