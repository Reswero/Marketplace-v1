using Marketplace.Products.Application.Common.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Commands.CreateDiscount;

/// <summary>
/// Команда создания скидки
/// </summary>
/// <param name="Size">Размер</param>
/// <param name="ValidUntil">Действительна до</param>
public record CreateDiscountCommand(int Size, DateOnly ValidUntil);

/// <summary>
/// Команда создания скидки
/// </summary>
/// <param name="ProductId">Идентификатор товара</param>
/// <param name="Size">Размер</param>
/// <param name="ValidUntil">Действительна до</param>
public record CreateDiscountWithProductIdCommand(int ProductId, int Size, DateOnly ValidUntil)
    :IRequest;
