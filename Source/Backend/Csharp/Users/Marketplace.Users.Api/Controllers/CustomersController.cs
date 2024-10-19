using Marketplace.Common.Authorization.Attributes;
using Marketplace.Common.Authorization.Extensions;
using Marketplace.Common.Authorization.Models;
using Marketplace.Common.Responses;
using Marketplace.Users.Application.Customers.Commands.CreateCustomer;
using Marketplace.Users.Application.Customers.Commands.UpdateCustomer;
using Marketplace.Users.Application.Customers.Queries.GetCustomer;
using Marketplace.Users.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Users.Api.Controllers;

/// <summary>
/// Управление покупателями
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("v1/customers")]
public class CustomersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Создание профиля покупателя
    /// </summary>
    [HttpPost("/internal/v1/customers")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCustomer(CreateCustomerCommand cmd)
    {
        await _mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Получение профиля покупателя
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [AccountTypeAuthorize(AccountType.Customer, AccountType.Staff, AccountType.Admin)]
    [HttpGet("{accountId:int}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomer(int accountId)
    {
        if (HttpContext.CheckAccessById(accountId) is false)
            return Forbid();

        var customer = await _mediator.Send(new GetCustomerQuery(accountId));
        return Ok(customer);
    }

    /// <summary>
    /// Обновление профиля покупателя
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [AccountTypeAuthorize(AccountType.Customer, AccountType.Staff, AccountType.Admin)]
    [HttpPut("{accountId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateCustomer(int accountId, UpdateCustomerCommand cmd)
    {
        if (HttpContext.CheckAccessById(accountId) is false)
            return Forbid();

        await _mediator.Send(cmd);
        return Ok();
    }
}
