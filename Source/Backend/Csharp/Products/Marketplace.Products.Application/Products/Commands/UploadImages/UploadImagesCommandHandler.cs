using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Domain.Products;
using MediatR;

namespace Marketplace.Products.Application.Products.Commands.UploadImages;

internal class UploadImagesCommandHandler(IProductsRepository repository, IProductsObjectStorage objectStorage,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UploadImagesCommand>
{
    private readonly IProductsRepository _repository = repository;
    private readonly IProductsObjectStorage _objectStorage = objectStorage;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UploadImagesCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetAsync(request.ProductId, cancellationToken);
        var imagesNames = await _objectStorage.UploadImagesAsync(request.Images, cancellationToken);

        var maxOrder = product.Images.MaxBy(p => p.Order)?.Order ?? default;
        var images = new Image[imagesNames.Count];

        for (int i = 0; i < imagesNames.Count; i++, maxOrder++)
        {
            Image image = new(product.Id, maxOrder + 1, ApplicationConsts.ProductsImagesBucket, imagesNames[i]);
            images[i] = image;
        }

        product.AddImages(images);

        await _repository.UpdateAsync(product, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
