using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Interfaces;
using MediatR;

namespace Marketplace.Products.Application.Products.Commands.DeleteImages;

/// <summary>
/// Удаление изображений товара
/// </summary>
internal class DeleteImagesCommandHandler(IProductsRepository repository, IProductsObjectStorage objectStorage,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteImagesWithProductIdCommand>
{
    private readonly IProductsRepository _repository = repository;
    private readonly IProductsObjectStorage _objectStorage = objectStorage;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteImagesWithProductIdCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetAsync(request.ProductId, cancellationToken);

        var imagesToDelete = product.Images.Where(i => request.ImagesIds.Contains(i.Id)).ToArray();
        product.RemoveImages(imagesToDelete);

        // TODO: Reoder images

        await _repository.UpdateAsync(product, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
        await _objectStorage.DeleteImagesAsync(imagesToDelete.Select(i => i.Name).ToList(), cancellationToken);
    }
}
