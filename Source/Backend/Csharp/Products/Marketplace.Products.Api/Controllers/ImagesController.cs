using Marketplace.Common.Authorization;
using Marketplace.Common.Authorization.Attributes;
using Marketplace.Common.Authorization.Extensions;
using Marketplace.Common.Authorization.Models;
using Marketplace.Products.Application.Common.DTOs;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Products.Commands.DeleteImages;
using Marketplace.Products.Application.Products.Commands.UploadImages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Marketplace.Products.Api.Controllers;

/// <summary>
/// Взаимодействие с изображениями товара
/// </summary>
/// <param name="mediator"></param>
/// <param name="accessChecker"></param>
[ApiController]
[Route("v1/proudcts")]
public class ImagesController(IMediator mediator, IProductsAccessChecker accessChecker) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IProductsAccessChecker _accessChecker = accessChecker;

    /// <summary>
    /// Загрузить изображения товара
    /// </summary>
    /// <param name="id">Идентификатор товара</param>
    /// <param name="files">Изображения</param>
    [AccountTypeAuthorize(AccountType.Seller)]
    [HttpPost("{id:int}/images")]
    public async Task<IActionResult> UploadImages(int id, IFormFile[] files)
    {
        var claims = HttpContext.GetClaims();
        if (await _accessChecker.CheckAccessAsync(id, claims.Id) is false)
        {
            return StatusCode(StatusCodes.Status403Forbidden, AuthorizationConsts.ForbidResponse);
        }

        List<FileDto> images = new(files.Length);
        foreach (var file in files)
        {
            if (file.ContentType != MediaTypeNames.Image.Jpeg)
                continue;

            images.Add(new(file.ContentType, file.FileName, file.OpenReadStream()));
        }

        await _mediator.Send(new UploadImagesCommand(id, images));
        return Ok();
    }

    /// <summary>
    /// Удалить изображения товара
    /// </summary>
    /// <param name="id">Идентификатор товара</param>
    /// <param name="cmd"></param>
    [AccountTypeAuthorize(AccountType.Seller, AccountType.Staff, AccountType.Admin)]
    [HttpDelete("{id:int}/images")]
    public async Task<IActionResult> DeleteImages(int id, DeleteImagesCommand cmd)
    {
        var claims = HttpContext.GetClaims();
        if (await _accessChecker.CheckAccessAsync(id, claims.Id) is false &&
            claims.Type == AccountType.Seller)
        {
            return StatusCode(StatusCodes.Status403Forbidden, AuthorizationConsts.ForbidResponse);
        }

        await _mediator.Send(new DeleteImagesWithProductIdCommand(id, cmd.ImagesIds));
        return Ok();
    }
}
