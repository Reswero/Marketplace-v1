using Marketplace.Products.Application.Common.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Commands.DeleteProduct;

/// <summary>
/// Команда удаления товара
/// </summary>
/// <param name="Id">Идентификатор товара</param>
public record DeleteProductCommand(int Id) : IRequest<DeleteObjectResultVM>;
