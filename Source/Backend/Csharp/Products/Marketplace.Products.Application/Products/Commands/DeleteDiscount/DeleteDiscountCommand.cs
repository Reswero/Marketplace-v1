using Marketplace.Products.Application.Common.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Commands.DeleteDiscount;

/// <summary>
/// Команда удаления скидки
/// </summary>
/// <param name="ProductId">Идентификатор товара</param>
/// <param name="ValidUntil">Действительна до</param>
public record DeleteDiscountCommand(int ProductId, DateOnly ValidUntil)
    :IRequest<DeleteObjectResultVM>;
