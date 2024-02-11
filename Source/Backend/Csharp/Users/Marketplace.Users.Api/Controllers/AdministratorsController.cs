using Marketplace.Users.Application.Administrators.Commands.CreateAdmin;
using Marketplace.Users.Application.Administrators.Commands.UpdateAdmin;
using Marketplace.Users.Application.Administrators.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Users.Api.Controllers;

[ApiController]
[Route("v1/administrators")]
public class AdministratorsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Создание профиля администратора
    /// </summary>
    [HttpPost("/internal/v1/administrators")]
    public async Task<IActionResult> CreateAdmin(CreateAdminCommand cmd)
    {
        await _mediator.Send(cmd);
        return Created();
    }

    /// <summary>
    /// Получение профиля администратора
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [HttpGet("{accountId:int}")]
    public async Task<IActionResult> GetAdmin(int accountId)
    {
        var admin = await _mediator.Send(new GetAdminQuery(accountId));
        return Ok(admin);
    }

    /// <summary>
    /// Обновление профиля администратора
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [HttpPut("{accountId:int}")]
    public async Task<IActionResult> UpdateAdmin(int accountId, UpdateAdminCommand cmd)
    {
        await _mediator.Send(cmd);
        return Ok();
    }
}
