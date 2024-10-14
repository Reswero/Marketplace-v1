using Marketplace.Common.SoftDelete.Extensions;
using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Common.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Products.Commands.DeleteProduct;

/// <summary>
/// Удаление товара
/// </summary>
/// <param name="repository"></param>
/// <param name="unitOfWork"></param>
internal class DeleteProductCommandHandler(IProductsRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProductCommand, DeleteObjectResultVM>
{
    private readonly IProductsRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeleteObjectResultVM> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetAsync(request.Id, cancellationToken);
        product.ThrowIfDeleted();

        await _repository.DeleteAsync(product, cancellationToken);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return new(result > 0);
    }
}
