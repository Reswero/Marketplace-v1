using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Exceptions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Common.ViewModels;
using Marketplace.Products.Domain.Categories;
using MediatR;

namespace Marketplace.Products.Application.Categories.Commands.CreateSubcategory;

/// <summary>
/// Создание подкатегории
/// </summary>
/// <param name="repository"></param>
/// <param name="unitOfWork"></param>
internal class CreateSubcategoryCommandHandler(ICategoriesRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateSubcategoryWithCategoryIdCommand, CreateObjectResultVM>
{
    private readonly ICategoriesRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateObjectResultVM> Handle(CreateSubcategoryWithCategoryIdCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetAsync(request.CategoryId, cancellationToken);
        if (category.Subсategories.Any(s => string.Equals(s.Name, request.Name, StringComparison.InvariantCultureIgnoreCase)))
        {
            throw new ObjectAlreadyExistsException(typeof(Subсategory), request.Name);
        }

        Subсategory subсategory = new(category.Id, request.Name);
        category.AddSubcategories(subсategory);

        await _repository.UpdateAsync(category, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new(subсategory.Id);
    }
}
