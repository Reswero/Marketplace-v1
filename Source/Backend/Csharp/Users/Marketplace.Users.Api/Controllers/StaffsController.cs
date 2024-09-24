using Marketplace.Common.Authorization.Extensions;
using Marketplace.Users.Application.Staffs.Commands.CreateStaff;
using Marketplace.Users.Application.Staffs.Commands.UpdateStaff;
using Marketplace.Users.Application.Staffs.Queries.GetStaff;
using Marketplace.Users.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Users.Api.Controllers;

/// <summary>
/// Управление персоналом
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("v1/staffs")]
public class StaffsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Создание профиля персонала
    /// </summary>
    [HttpPost("/internal/v1/staffs")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult>CreateStaff(CreateStaffCommand cmd)
    {
        await _mediator.Send(cmd);
        return Created();
    }

    /// <summary>
    /// Получение профиля персонала
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [HttpGet("{accountId:int}")]
    [ProducesResponseType(typeof(Staff), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult>GetStaff(int accountId)
    {
        if (HttpContext.CheckAccessById(accountId))
            return Forbid();

        var staff = await _mediator.Send(new GetStaffQuery(accountId));
        return Ok(staff);
    }

    /// <summary>
    /// Обновление профиля персонала
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [HttpPut("{accountId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult>UpdateStaff(int accountId, UpdateStaffCommand cmd)
    {
        if (HttpContext.CheckAccessById(accountId))
            return Forbid();

        await _mediator.Send(cmd);
        return Ok();
    }
}
