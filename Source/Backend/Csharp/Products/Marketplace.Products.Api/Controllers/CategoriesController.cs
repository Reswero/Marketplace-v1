using Marketplace.Common.Authorization.Attributes;
using Marketplace.Common.Authorization.Models;
using Marketplace.Common.Responses;
using Marketplace.Products.Application.Categories.Commands.CreateCategory;
using Marketplace.Products.Application.Categories.Commands.DeleteCategory;
using Marketplace.Products.Application.Categories.Commands.UpdateCategory;
using Marketplace.Products.Application.Categories.Queries.GetCategory;
using Marketplace.Products.Application.Categories.ViewModels;
using Marketplace.Products.Application.Common.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Products.Api.Controllers;

/// <summary>
/// Взаимодействие с категориями
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("v1/categories")]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Создание категории
    /// </summary>
    /// <param name="cmd"></param>
    [AccountTypeAuthorize(AccountType.Staff, AccountType.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(CreateObjectResultVM), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(CreateCategoryCommand cmd)
    {
        var result = await _mediator.Send(cmd);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// Получение категории
    /// </summary>
    /// <param name="id">Идентификатор</param>
    [AccountTypeAuthorize(AccountType.Seller, AccountType.Admin)]
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoryVM), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var category = await _mediator.Send(new GetCategoryQuery(id));
        return Ok(category);
    }

    /// <summary>
    /// Обновление категории
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cmd"></param>
    [AccountTypeAuthorize(AccountType.Staff, AccountType.Admin)]
    [HttpPost("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateCategoryCommand cmd)
    {
        await _mediator.Send(new UpdateCategoryWithIdCommand(id, cmd.Name, cmd.Parameters));
        return Ok();
    }

    /// <summary>
    /// Удаление категории
    /// </summary>
    /// <param name="id">Идентификатор</param>
    [AccountTypeAuthorize(AccountType.Admin)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteCategoryCommand(id));
        return Ok(result);
    }
}
