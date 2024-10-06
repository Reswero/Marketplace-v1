﻿using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Exceptions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Domain.Categories;
using MediatR;

namespace Marketplace.Products.Application.Categories.Commands.UpdateSubcategory;

/// <summary>
/// Обновление подкатегории
/// </summary>
/// <param name="repository"></param>
/// <param name="unitOfWork"></param>
internal class UpdateSubcategoryCommandHandler(ICategoriesRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateSubcategoryWithCategoryIdCommand>
{
    private readonly ICategoriesRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateSubcategoryWithCategoryIdCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetAsync(request.CategoryId, cancellationToken);

        var subcategory = category.Subсategories.FirstOrDefault(s => s.Id == request.Id)
            ?? throw new ObjectNotFoundException(typeof(Subсategory), request.Id);

        subcategory.SetName(request.Name);

        await _repository.UpdateAsync(category, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}