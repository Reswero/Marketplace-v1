using Marketplace.Common.Authorization.Attributes;
using Marketplace.Common.Authorization.Models;
using Marketplace.Common.Responses;
using Marketplace.Orders.Application.Orders.Commands.PackOrderProduct;
using Marketplace.Orders.Application.Orders.Queries.GetSellerOrders;
using Marketplace.Orders.Application.Orders.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Orders.Api.Controllers;

/// <summary>
/// Взаимодействие с заказами продавцов
/// </summary>
[ApiController]
[Route("v1/seller")]
public class SellerOrdersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Получение заказов продавца
    /// </summary>
    /// <param name="query"></param>
    [HttpGet("orders")]
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
    [HttpPost("orders/{orderProductId:int}/pack")]
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
