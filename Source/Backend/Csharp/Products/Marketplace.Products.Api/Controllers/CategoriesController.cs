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
/// Управление категориями
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
    [HttpPost]
    public Task<CreateObjectResultVM> Add(CreateCategoryCommand cmd)
        => _mediator.Send(cmd);

    /// <summary>
    /// Получение категории
    /// </summary>
    /// <param name="id">Идентификатор</param>
    [HttpGet("{id:int}")]
    public Task<CategoryVM> Get(int id)
        => _mediator.Send(new GetCategoryQuery(id));

    /// <summary>
    /// Обновление категории
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cmd"></param>
    [HttpPost("{id:int}")]
    public Task Update(int id, UpdateCategoryCommand cmd)
        => _mediator.Send(cmd);

    /// <summary>
    /// Удаление категории
    /// </summary>
    /// <param name="id">Идентификатор</param>
    [HttpDelete("{id:int}")]
    public Task<DeleteObjectResultVM> Delete(int id)
        => _mediator.Send(new DeleteCategoryCommand(id));
}
