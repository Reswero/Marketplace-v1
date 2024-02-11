using Marketplace.Users.Application.Sellers.Commands.CreateSeller;
using Marketplace.Users.Application.Sellers.Commands.UpdateSeller;
using Marketplace.Users.Application.Sellers.Queries.GetSeller;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Users.Api.Controllers;

[ApiController]
[Route("v1/sellers")]
public class SellersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Создание профиля продавца
    /// </summary>
    [HttpPost("/internal/v1/sellers")]
    public async Task<IActionResult> CreateSeller(CreateSellerCommand cmd)
    {
        await _mediator.Send(cmd);
        return Created();
    }

    /// <summary>
    /// Получение профиля продавца
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [HttpGet("{accountId:int}")]
    public async Task<IActionResult> GetSeller(int accountId)
    {
        var seller = await _mediator.Send(new GetSellerQuery(accountId));
        return Ok(seller);
    }

    /// <summary>
    /// Обновление профиля продавца
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [HttpPut("{accountId:int}")]
    public async Task<IActionResult> UpdateSeller(int accountId, UpdateSellerCommand cmd)
    {
        await _mediator.Send(cmd);
        return Ok();
    }
}
