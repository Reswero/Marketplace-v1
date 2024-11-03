namespace Marketplace.Products.Infrastructure.Products.Models;

/// <summary>
/// Изображение для операции удаления
/// </summary>
/// <param name="Bucket">Название бакета</param>
/// <param name="Name">Название изображения</param>
public record ImageToDelete(string Bucket, string Name);
