﻿using Marketplace.Common.Authorization.Attributes;
using Marketplace.Common.Authorization.Extensions;
using Marketplace.Common.Authorization.Models;
using Marketplace.Common.Responses;
using Marketplace.Users.Application.Sellers.Commands.CreateSeller;
using Marketplace.Users.Application.Sellers.Commands.UpdateSeller;
using Marketplace.Users.Application.Sellers.Queries.GetSeller;
using Marketplace.Users.Application.Sellers.Queries.GetSellerShortInfo;
using Marketplace.Users.Application.Sellers.ViewModels;
using Marketplace.Users.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Users.Api.Controllers;

/// <summary>
/// Управление продавцами
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("v1/sellers")]
public class SellersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Создание профиля продавца
    /// </summary>
    [HttpPost("/internal/v1/sellers")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateSeller(CreateSellerCommand cmd)
    {
        await _mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Получение профиля продавца
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [AccountTypeAuthorize(AccountType.Seller, AccountType.Staff, AccountType.Admin)]
    [HttpGet("{accountId:int}")]
    [ProducesResponseType(typeof(Seller), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSeller(int accountId)
    {
        if (HttpContext.CheckAccessById(accountId) is false)
            return Forbid();

        var seller = await _mediator.Send(new GetSellerQuery(accountId));
        return Ok(seller);
    }

    /// <summary>
    /// Получение краткой информации о продавце
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [HttpGet("{accountId:int}/info")]
    [ProducesResponseType(typeof(SellerShortInfoVM), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSellerShortInfo(int accountId)
    {
        var info = await _mediator.Send(new GetSellerShortInfoQuery(accountId));
        return Ok(info);
    }

    /// <summary>
    /// Обновление профиля продавца
    /// </summary>
    /// <param name="accountId">Идентификатор аккаунта</param>
    [AccountTypeAuthorize(AccountType.Seller, AccountType.Staff, AccountType.Admin)]
    [HttpPut("{accountId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateSeller(int accountId, UpdateSellerCommand cmd)
    {
        if (HttpContext.CheckAccessById(accountId) is false)
            return Forbid();
        
        await _mediator.Send(cmd);
        return Ok();
    }
}
