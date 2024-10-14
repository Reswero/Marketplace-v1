using Marketplace.Common.SoftDelete.Extensions;
using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Exceptions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Common.ViewModels;
using Marketplace.Products.Domain.Products;
using MediatR;

namespace Marketplace.Products.Application.Products.Commands.DeleteDiscount;

internal class DeleteDiscountCommandHandler(IProductsRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteDiscountCommand, DeleteObjectResultVM>
{
    private readonly IProductsRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeleteObjectResultVM> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetAsync(request.ProductId, cancellationToken);
        product.ThrowIfDeleted();

        var discount = product.Discounts.FirstOrDefault(d => d.ValidUntil == request.ValidUntil) ??
            throw new ObjectNotFoundException(typeof(Discount));
        product.RemoveDiscount(discount);

        await _repository.UpdateAsync(product, cancellationToken);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return new(result > 0);
    }
}
