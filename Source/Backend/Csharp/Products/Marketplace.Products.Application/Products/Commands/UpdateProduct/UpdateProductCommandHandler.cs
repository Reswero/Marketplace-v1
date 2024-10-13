using Marketplace.Common.Transactions;
using Marketplace.Products.Application.Common.Exceptions;
using Marketplace.Products.Application.Common.Interfaces;
using Marketplace.Products.Domain.Categories;
using Marketplace.Products.Domain.Parameters;
using MediatR;

namespace Marketplace.Products.Application.Products.Commands.UpdateProduct;

internal class UpdateProductCommandHandler(IProductsRepository productsRepository,
    ICategoriesRepository categoriesRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProductWithIdCommand>
{
    private readonly IProductsRepository _productsRepository = productsRepository;
    private readonly ICategoriesRepository _categoriesRepository = categoriesRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateProductWithIdCommand request, CancellationToken cancellationToken)
    {
        var product = await _productsRepository.GetAsync(request.Id, cancellationToken);
        product.ChangeInfo(request.Name, request.Description, request.Price);

        var category = await _categoriesRepository.GetAsync(product.CategoryId, cancellationToken);
        var subcategory = category.Subсategories.FirstOrDefault(s => s.Id == request.SubcategoryId)
            ?? throw new ObjectNotFoundException(typeof(Subсategory), request.SubcategoryId);

        product.ChangeSubcategory(subcategory);

        var categoryParametersIds = category.Parameters.Select(p => p.Id).ToHashSet();
        var parametersIds = product.Parameters.Select(p => p.Id).ToHashSet();

        var unexistingParameters = request.Parameters.Where(p => p.Id == 0 &&
            categoryParametersIds.Contains(p.CategoryParameterId));
        var existingParameters = request.Parameters.Where(p => parametersIds.Contains(p.Id) &&
            categoryParametersIds.Contains(p.CategoryParameterId));

        var parameters = existingParameters.Union(unexistingParameters)
            .Select(p => new ProductParameter(p.Id, p.CategoryParameterId, product.Id, p.Value))
            .ToArray();

        product.RemoveParameters([.. product.Parameters]);
        product.AddParameters(parameters);

        await _productsRepository.UpdateAsync(product, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}
