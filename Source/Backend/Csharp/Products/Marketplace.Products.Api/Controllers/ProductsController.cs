using Marketplace.Common.Authorization;
using Marketplace.Common.Authorization.Attributes;
using Marketplace.Common.Authorization.Extensions;
using Marketplace.Common.Authorization.Models;
using Marketplace.Common.Responses;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Common.ViewModels;
using Marketplace.Products.Application.Products.Commands.CreateProduct;
using Marketplace.Products.Application.Products.Commands.DeleteProduct;
using Marketplace.Products.Application.Products.Commands.UpdateProduct;
using Marketplace.Products.Application.Products.DTOs;
using Marketplace.Products.Application.Products.Queries.CheckProductsExistence;
using Marketplace.Products.Application.Products.Queries.GetProduct;
using Marketplace.Products.Application.Products.Queries.GetProductFullInfo;
using Marketplace.Products.Application.Products.Queries.GetProductsByIds;
using Marketplace.Products.Application.Products.Queries.GetProductsByIdsInternal;
using Marketplace.Products.Application.Products.Queries.SearchProductsQuery;
using Marketplace.Products.Application.Products.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Products.Api.Controllers;

/// <summary>
/// Взаимодействие с товарами
/// </summary>
/// <param name="mediator"></param>
[ApiController]
[Route("v1/products")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    /// <summary>
    /// Создание товара
    /// </summary>
    /// <param name="cmd"></param>
    [AccountTypeAuthorize(AccountType.Seller)]
    [HttpPost]
    [ProducesResponseType(typeof(CreateObjectResultVM), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create(CreateProductCommand cmd)
    {
        var claims = HttpContext.GetClaims();

        var result = await _mediator.Send(new CreateProductWithSellerIdCommand(claims.Id, cmd.CategoryId,
            cmd.SubcategoryId, cmd.Name, cmd.Description, cmd.Price, cmd.Parameters));
        return StatusCode(StatusCodes.Status201Created, result);
    }

    /// <summary>
    /// Получение товара
    /// </summary>
    /// <param name="id">Идентификатор товара</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProductVM), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var product = await _mediator.Send(new GetProductQuery(id));
        return Ok(product);
    }

    /// <summary>
    /// Получение полной информации о товаре
    /// </summary>
    /// <param name="id">Идентификатор товара</param>
    /// <param name="accessChecker"></param>
    [AccountTypeAuthorize(AccountType.Seller, AccountType.Staff, AccountType.Admin)]
    [HttpGet("{id:int}/full-info")]
    [ProducesResponseType(typeof(ProductFullInfoVM), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFullInfo(int id, [FromServices] IProductsAccessChecker accessChecker)
    {
        var checkResult = await CheckAccessToProductAsync(accessChecker, id);
        if (checkResult is not null)
        {
            return checkResult;
        }

        var product = await _mediator.Send(new GetProductFullInfoQuery(id));
        return Ok(product);
    }

    /// <summary>
    /// Получение краткой информации о товарах по идентификаторам
    /// </summary>
    /// <param name="query"></param>
    [HttpGet("by-ids")]
    [ProducesResponseType(typeof(List<ProductShortInfoVM>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIds([FromQuery] GetProductsByIdsQuery query)
    {
        var products = await _mediator.Send(query);
        return Ok(products);
    }

    /// <summary>
    /// Поиск товаров
    /// </summary>
    /// <param name="query"></param>
    [HttpPost("search")]
    [ProducesResponseType(typeof(SearchProductsResultVM), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search(SearchProductsQuery query)
    {
        var products = await _mediator.Send(query);
        return Ok(products);
    }

    /// <summary>
    /// Обновление товара
    /// </summary>
    /// <param name="id">Идентификатор товара</param>
    /// <param name="cmd"></param>
    /// <param name="accessChecker"></param>
    [AccountTypeAuthorize(AccountType.Seller, AccountType.Staff, AccountType.Admin)]
    [HttpPost("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateProductCommand cmd,
        [FromServices] IProductsAccessChecker accessChecker)
    {
        var checkResult = await CheckAccessToProductAsync(accessChecker, id);
        if (checkResult is not null)
        {
            return checkResult;
        }

        await _mediator.Send(new UpdateProductWithIdCommand(id, cmd.SubcategoryId, cmd.Name,
            cmd.Description, cmd.Price, cmd.Parameters));

        return Ok();
    }

    /// <summary>
    /// Удаление товара
    /// </summary>
    /// <param name="id">Идентификатор товара</param>
    /// <param name="accessChecker"></param>
    [AccountTypeAuthorize(AccountType.Seller, AccountType.Staff, AccountType.Admin)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(DeleteObjectResultVM), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, [FromServices] IProductsAccessChecker accessChecker)
    {
        var checkResult = await CheckAccessToProductAsync(accessChecker, id);
        if (checkResult is not null)
        {
            return checkResult;
        }

        var result = await _mediator.Send(new DeleteProductCommand(id));
        return Ok(result);
    }

    /// <summary>
    /// Получение краткой информации о товарах по идентификаторам
    /// </summary>
    /// <param name="query"></param>
    [HttpGet("/internal/v1/products/by-ids")]
    [ProducesResponseType(typeof(List<ProductShortInfoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdsInternal([FromQuery] GetProductsByIdsInternalQuery query)
    {
        var products = await _mediator.Send(query);
        return Ok(products);
    }

    /// <summary>
    /// Проверка существования товаров
    /// </summary>
    /// <param name="query"></param>
    [HttpGet("/internal/v1/products/existence")]
    [ProducesResponseType(typeof(ExistingProductsVM), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CheckExistence([FromQuery] CheckProductsExistenceQuery query)
    {
        var ids = await _mediator.Send(query);
        return Ok(ids);
    }

    private async Task<ObjectResult?> CheckAccessToProductAsync(IProductsAccessChecker accessChecker, int productId)
    {
        var claims = HttpContext.GetClaims();
        if (await accessChecker.CheckAccessAsync(productId, claims.Id) is false &&
            claims.Type == AccountType.Seller)
        {
            return StatusCode(StatusCodes.Status403Forbidden, AuthorizationConsts.ForbidResponse);
        }

        return null;
    }
}
