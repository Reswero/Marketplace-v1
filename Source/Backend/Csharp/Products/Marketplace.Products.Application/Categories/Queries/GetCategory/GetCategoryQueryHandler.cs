using Marketplace.Products.Application.Categories.ViewModels;
using Marketplace.Products.Application.Common.Interfaces;
using MediatR;

namespace Marketplace.Products.Application.Categories.Queries.GetCategory;

/// <summary>
/// Получение категории
/// </summary>
/// <param name="repository"></param>
internal class GetCategoryQueryHandler(ICategoriesRepository repository)
    : IRequestHandler<GetCategoryQuery, CategoryVM>
{
    private readonly ICategoriesRepository _repository = repository;

    public async Task<CategoryVM> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _repository.GetAsync(request.Id, cancellationToken);

        var parameters = category.Parameters.OrderBy(p => p.Name)
            .Select(p => new CategoryParameterVM(p.Id, p.Name, p.Type))
            .ToList();
        var subcategories = category.Subсategories.OrderBy(s => s.Name)
            .Select(s => new SubcategoryVM(s.Id, s.Name))
            .ToList();

        return new(category.Id, category.Name, parameters, subcategories);
    }
}
