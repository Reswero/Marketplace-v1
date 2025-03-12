using Marketplace.Common.Responses;
using Marketplace.Orders.Application.Common.ViewModels;
using Marketplace.Orders.Application.Orders.Commands.CreateOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Orders.Api.Controllers;

/// <summary>
/// Взаимодействие с заказами
/// </summary>
[ApiController]
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
}
