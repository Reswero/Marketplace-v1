namespace Marketplace.Products.Domain.Exceptions;

/// <summary>
/// Превышен лимит изображений
/// </summary>
public class ImagesLimitExceededException()
    : Exception("Превышен лимит изображений")
{ }
