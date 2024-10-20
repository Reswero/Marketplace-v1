namespace Marketplace.Products.Domain.Products;

/// <summary>
/// Изображение товара
/// </summary>
public class Image
{
    private Image() { }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="productId">Идентификатор товара</param>
    /// <param name="order">Порядок</param>
    /// <param name="bucketName">Название bucket'а</param>
    /// <param name="name">Название изображения</param>
    public Image(int productId, int order, string bucketName, string name)
    {
        ProductId = productId;
        Order = order;
        BucketName = bucketName;
        Name = name;
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Идентификатор товара
    /// </summary>
    public int ProductId { get; set; }
    /// <summary>
    /// Порядок
    /// </summary>
    public int Order { get; set; }
    /// <summary>
    /// Название bucket'а
    /// </summary>
    public string BucketName { get; set; } = null!;
    /// <summary>
    /// Название изображения
    /// </summary>
    public string Name { get; set; } = null!;
}
