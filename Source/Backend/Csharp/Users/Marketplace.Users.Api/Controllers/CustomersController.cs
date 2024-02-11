using Marketplace.Users.Application.Customers.Commands.CreateCustomer;
using Marketplace.Users.Application.Customers.Commands.UpdateCustomer;
using Marketplace.Users.Application.Customers.Queries.GetCustomer;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Users.Api.Controllers;

[ApiController]
[Route("v1/customers")]
public class CustomersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Создание профиля покупателя
    /// </summary>
    [HttpPost("/internal/v1/customers")]
    public async Task<IActionResult> CreateCustomer(CreateCustomerCommand cmd)
    {
        await _mediator.Send(cmd);
        return Created();
    }

    /// <summary>
    /// Получение профиля покупателя
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [HttpGet("{accountId:int}")]
    public async Task<IActionResult> GetCustomer(int accountId)
    {
        var customer = await _mediator.Send(new GetCustomerQuery(accountId));
        return Ok(customer);
    }

    /// <summary>
    /// Обновление профиля покупателя
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [HttpPut("{accountId:int}")]
    public async Task<IActionResult> UpdateCustomer(int accountId, UpdateCustomerCommand cmd)
    {
        await _mediator.Send(cmd);
        return Ok();
    }
}
