namespace Marketplace.Products.Application.Categories.ViewModels;

/// <summary>
/// Модель категории с подкатегориями
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="Name">Название</param>
/// <param name="Subcategories">Подкатегории</param>
public record CategoryWithSubcategoriesVM(int Id, string Name, List<SubcategoryVM> Subcategories);
