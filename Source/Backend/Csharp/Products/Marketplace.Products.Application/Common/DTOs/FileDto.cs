namespace Marketplace.Products.Application.Common.DTOs;

/// <summary>
/// Файл
/// </summary>
/// <param name="ContentType">Тип контента</param>
/// <param name="Name">Название</param>
/// <param name="Stream">Поток с данными</param>
public record FileDto(string ContentType, string Name, Stream Stream);
