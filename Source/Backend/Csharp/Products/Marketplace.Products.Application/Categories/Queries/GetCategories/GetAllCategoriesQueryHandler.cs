using Marketplace.Products.Application.Categories.ViewModels;
using Marketplace.Products.Application.Common.Interfaces;
using MediatR;

namespace Marketplace.Products.Application.Categories.Queries.GetCategories;

/// <summary>
/// Получение всех категорий
/// </summary>
/// <param name="repository"></param>
internal class GetAllCategoriesQueryHandler(ICategoriesRepository repository)
    : IRequestHandler<GetAllCategoriesQuery, List<CategoryWithSubcategoriesVM>>
{
    private readonly ICategoriesRepository _repository = repository;

    public async Task<List<CategoryWithSubcategoriesVM>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetAllAsync(cancellationToken);

        return categories.OrderBy(c => c.Name)
            .Select(c =>
            {
                var subcategories = c.Subсategories.OrderBy(s => s.Name)
                    .Select(s => new SubcategoryVM(s.Id, s.Name))
                    .ToList();

                return new CategoryWithSubcategoriesVM(c.Id, c.Name, subcategories);
            }).ToList();
    }
}
