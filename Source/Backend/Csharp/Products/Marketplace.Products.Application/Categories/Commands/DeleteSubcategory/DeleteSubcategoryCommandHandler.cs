using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Exceptions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Common.ViewModels;
using Marketplace.Products.Domain.Categories;
using MediatR;

namespace Marketplace.Products.Application.Categories.Commands.DeleteSubcategory;

/// <summary>
/// Удаление подкатегории
/// </summary>
/// <param name="repository"></param>
/// <param name="unitOfWork"></param>
internal class DeleteSubcategoryCommandHandler(ICategoriesRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteSubcategoryCommand, DeleteObjectResultVM>
{
    private readonly ICategoriesRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeleteObjectResultVM> Handle(DeleteSubcategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetAsync(request.CategoryId, cancellationToken);

        var subcategory = category.Subсategories.FirstOrDefault(s => s.Id == request.Id)
            ?? throw new ObjectNotFoundException(typeof(Subсategory), request.Id);

        category.RemoveSubcategories(subcategory);

        await _repository.UpdateAsync(category, cancellationToken);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return new(result > 0);
    }
}
