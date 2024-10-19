namespace Marketplace.Products.Application.Categories.ViewModels;

/// <summary>
/// Модель категории
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="Name">Название</param>
/// <param name="Parameters">Параметры</param>
/// <param name="Subcategories">Подкатегории</param>
public record CategoryVM(
    int Id,
    string Name,
    List<CategoryParameterVM> Parameters,
    List<SubcategoryVM> Subcategories
);
