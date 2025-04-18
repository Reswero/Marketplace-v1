﻿using Marketplace.Products.Domain.Parameters;

namespace Marketplace.Products.Application.Categories.ViewModels;

/// <summary>
/// Модель обновления параметра категории
/// </summary>
/// <param name="Id">Идентификатор</param>
/// <param name="Name">Название</param>
/// <param name="Type">Тип</param>
public record UpdateCategoryParameterVM(int Id, string Name, ParameterType Type);
