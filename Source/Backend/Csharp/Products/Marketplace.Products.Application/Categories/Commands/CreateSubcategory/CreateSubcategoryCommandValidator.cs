﻿using FluentValidation;

namespace Marketplace.Products.Application.Categories.Commands.CreateSubcategory;

public class CreateSubcategoryCommandValidator : AbstractValidator<CreateSubcategoryCommand>
{
    public CreateSubcategoryCommandValidator()
    {
        RuleFor(s => s.Name).MinimumLength(2).MaximumLength(100);
    }
}
