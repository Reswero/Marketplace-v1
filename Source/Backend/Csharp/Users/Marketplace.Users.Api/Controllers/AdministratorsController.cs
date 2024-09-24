using Marketplace.Common.Authorization.Attributes;
using Marketplace.Common.Authorization.Models;
using Marketplace.Common.Responses;
using Marketplace.Users.Application.Administrators.Commands.CreateAdmin;
using Marketplace.Users.Application.Administrators.Commands.UpdateAdmin;
using Marketplace.Users.Application.Administrators.Queries.GetAdmin;
using Marketplace.Users.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Users.Api.Controllers;

/// <summary>
/// Управление администраторами
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("v1/administrators")]
public class AdministratorsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Создание профиля администратора
    /// </summary>
    /// <param name="cmd"></param>
    [HttpPost("/internal/v1/administrators")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAdmin(CreateAdminCommand cmd)
    {
        await _mediator.Send(cmd);
        return Created();
    }

    /// <summary>
    /// Получение профиля администратора
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [AccountTypeAuthorize(AccountType.Admin)]
    [HttpGet("{accountId:int}")]
    [ProducesResponseType(typeof(Administrator), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAdmin(int accountId)
    {
        var admin = await _mediator.Send(new GetAdminQuery(accountId));
        return Ok(admin);
    }

    /// <summary>
    /// Обновление профиля администратора
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    /// <param name="cmd"></param>
    [AccountTypeAuthorize(AccountType.Admin)]
    [HttpPut("{accountId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateAdmin(int accountId, UpdateAdminCommand cmd)
    {
        await _mediator.Send(cmd);
        return Ok();
    }
}
