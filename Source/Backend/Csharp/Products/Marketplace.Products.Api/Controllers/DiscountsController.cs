using Marketplace.Common.Authorization.Extensions;
using Marketplace.Common.Authorization.Models;
using Marketplace.Common.Authorization;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Products.Commands.CreateDiscount;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Marketplace.Common.Authorization.Attributes;
using Marketplace.Products.Application.Products.Commands.DeleteDiscount;
using Marketplace.Products.Application.Common.ViewModels;

namespace Marketplace.Products.Api.Controllers;

/// <summary>
/// Взаимодейтсвие со скидками
/// </summary>
/// <param name="mediator"></param>
/// <param name="accessChecker"></param>
[ApiController]
[Route("v1/products")]
public class DiscountsController(IMediator mediator, IProductsAccessChecker accessChecker)
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IProductsAccessChecker _accessChecker = accessChecker;

    /// <summary>
    /// Создание скидки
    /// </summary>
    /// <param name="id">Идентификатор товара</param>
    /// <param name="cmd"></param>
    [AccountTypeAuthorize(AccountType.Seller)]
    [HttpPost("{id:int}/discounts")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(int id, CreateDiscountCommand cmd)
    {
        var claims = HttpContext.GetClaims();
        if (await _accessChecker.CheckAccessAsync(id, claims.Id) is false)
        {
            return StatusCode(StatusCodes.Status403Forbidden, AuthorizationConsts.ForbidResponse);
        }

        await _mediator.Send(new CreateDiscountWithProductIdCommand(id, cmd.Size, cmd.ValidUntil));
        return StatusCode(StatusCodes.Status201Created);
    }

    /// <summary>
    /// Удаление скидки
    /// </summary>
    /// <param name="id">Идентификатор товара</param>
    /// <param name="validUntil">Действительна до</param>
    [AccountTypeAuthorize(AccountType.Seller, AccountType.Staff, AccountType.Admin)]
    [HttpDelete("{id:int}/discounts")]
    [ProducesResponseType(typeof(DeleteObjectResultVM), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, [FromQuery] DateOnly validUntil)
    {
        var claims = HttpContext.GetClaims();
        if (await _accessChecker.CheckAccessAsync(id, claims.Id) is false &&
            claims.Type == AccountType.Seller)
        {
            return StatusCode(StatusCodes.Status403Forbidden, AuthorizationConsts.ForbidResponse);
        }

        var result = await _mediator.Send(new DeleteDiscountCommand(id, validUntil));
        return Ok(result);
    }
}
