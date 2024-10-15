using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Exceptions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Application.Common.ViewModels;
using Marketplace.Products.Domain.Categories;
using Marketplace.Products.Domain.Parameters;
using MediatR;

namespace Marketplace.Products.Application.Categories.Commands.CreateCategory;

/// <summary>
/// Создание категории
/// </summary>
/// <param name="repository"></param>
/// <param name="unitOfWork"></param>
internal class CreateCategoryCommandHandler(ICategoriesRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCategoryCommand, CreateObjectResultVM>
{
    private readonly ICategoriesRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<CreateObjectResultVM> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetAllAsync(cancellationToken);
        if (categories.Any(c => string.Equals(c.Name, request.Name, StringComparison.InvariantCultureIgnoreCase)))
        {
            throw new ObjectAlreadyExistsException(typeof(Category), request.Name);
        }

        Category category = new(request.Name);

        var parameters = new CategoryParameter[request.Parameters.Count];
        for (int i = 0; i < request.Parameters.Count; i++)
        {
            var vm = request.Parameters[i];
            CategoryParameter parameter = new(category, vm.Name, vm.Type);
            parameters[i] = parameter;
        }

        category.AddParameters(parameters);

        await _repository.AddAsync(category, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new(category.Id);
    }
}
