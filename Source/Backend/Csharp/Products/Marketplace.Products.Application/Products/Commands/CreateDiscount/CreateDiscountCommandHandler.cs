﻿using Marketplace.Common.SoftDelete.Extensions;
using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Domain.Products;
using MediatR;

namespace Marketplace.Products.Application.Products.Commands.CreateDiscount;

/// <summary>
/// Создание скидки
/// </summary>
/// <param name="repository"></param>
/// <param name="unitOfWork"></param>
internal class CreateDiscountCommandHandler(IProductsRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateDiscountWithProductIdCommand>
{
    private readonly IProductsRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateDiscountWithProductIdCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetAsync(request.ProductId, cancellationToken);
        product.ThrowIfDeleted();

        Discount discount = new(product.Id, request.Size, request.ValidUntil);
        product.AddDiscount(discount);

        await _repository.UpdateAsync(product, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
