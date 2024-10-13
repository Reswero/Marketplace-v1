using Marketplace.Products.Application.Categories.ViewModels;

namespace Marketplace.Products.Application.Products.ViewModels;

internal class ProductVM
{
    public int Id { get; set; }
    public SellerVM Seller { get; set; } = null!;
    public ShortCategoryVM Category { get; set; } = null!;
    public SubcategoryVM Subcategory { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int Price { get; set; }
    public DiscountVM? Discount { get; set; }
    public List<ProductParameterVM> Parameters { get; set; } = null!;
}
