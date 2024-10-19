using Marketplace.Common.Authorization.Attributes;
using Marketplace.Common.Authorization.Models;
using Marketplace.Common.Responses;
using Marketplace.Products.Application.Categories.Commands.CreateSubcategory;
using Marketplace.Products.Application.Categories.Commands.DeleteSubcategory;
using Marketplace.Products.Application.Categories.Commands.UpdateSubcategory;
using Marketplace.Products.Application.Common.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Products.Api.Controllers;

/// <summary>
/// Взаимодействие с подкатегориями
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("v1/categories")]
public class SubcategoriesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Создание подкатегории
    /// </summary>
    /// <param name="categoryId">Идентификатор категории</param>
    /// <param name="cmd"></param>
    [AccountTypeAuthorize(AccountType.Staff, AccountType.Admin)]
    [HttpPost("{categoryId:int}/subcategories")]
    [ProducesResponseType(typeof(CreateObjectResultVM), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create(int categoryId, CreateSubcategoryCommand cmd)
    {
        var result = await _mediator.Send(new CreateSubcategoryWithCategoryIdCommand(categoryId, cmd.Name));
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// Обновление подкатегории
    /// </summary>
    /// <param name="categoryId">Идентификатор категории</param>
    /// <param name="id">Идентификатор подкатегории</param>
    /// <param name="cmd"></param>
    [AccountTypeAuthorize(AccountType.Staff, AccountType.Admin)]
    [HttpPost("{categoryId:int}/subcategories/{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int categoryId, int id, UpdateSubcategoryCommand cmd)
    {
        await _mediator.Send(new UpdateSubcategoryWithIdCommand(categoryId, id, cmd.Name));
        return Ok();
    }

    /// <summary>
    /// Удаление подкатегории
    /// </summary>
    /// <param name="categoryId">Идентификатор категории</param>
    /// <param name="id">Идентификатор подкатегории</param>
    /// <returns></returns>
    [AccountTypeAuthorize(AccountType.Admin)]
    [HttpDelete("{categoryId:int}/subcategories/{id:int}")]
    [ProducesResponseType(typeof(DeleteObjectResultVM), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int categoryId, int id)
    {
        var result = await _mediator.Send(new DeleteSubcategoryCommand(categoryId, id));
        return Ok(result);
    }
}
