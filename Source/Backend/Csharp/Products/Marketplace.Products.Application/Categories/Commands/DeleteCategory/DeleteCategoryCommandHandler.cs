using Marketplace.Common.SoftDelete.Extensions;
using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Common.ViewModels;
using MediatR;

namespace Marketplace.Products.Application.Categories.Commands.DeleteCategory;

/// <summary>
/// Удаление категории
/// </summary>
/// <param name="repository"></param>
/// <param name="unitOfWork"></param>
internal class DeleteCategoryCommandHandler(ICategoriesRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteCategoryCommand, DeleteObjectResultVM>
{
    private readonly ICategoriesRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<DeleteObjectResultVM> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetAsync(request.Id, cancellationToken);
        category.ThrowIfDeleted();

        await _repository.DeleteAsync(category, cancellationToken);
        var result = await _unitOfWork.CommitAsync(cancellationToken);

        return new(result > 0);
    }
}
