using Marketplace.Users.Application.Staffs.Commands.CreateStaff;
using Marketplace.Users.Application.Staffs.Commands.UpdateStaff;
using Marketplace.Users.Application.Staffs.Queries.GetStaff;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Users.Api.Controllers;

[ApiController]
[Route("v1/staffs")]
public class StaffsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Создание профиля персонала
    /// </summary>
    [HttpPost("/internal/v1/staffs")]
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
    public async Task<IActionResult>GetStaff(int accountId)
    {
        var staff = await _mediator.Send(new GetStaffQuery(accountId));
        return Ok(staff);
    }

    /// <summary>
    /// Обновление профиля персонала
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [HttpPut("{accountId:int}")]
    public async Task<IActionResult>UpdateStaff(int accountId, UpdateStaffCommand cmd)
    {
        await _mediator.Send(cmd);
        return Ok();
    }
}
