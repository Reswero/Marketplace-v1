﻿using FluentValidation;
using Marketplace.Products.Application.Categories.ViewModels.Validators;

namespace Marketplace.Products.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(c => c.Name).Length(2, 100);
        RuleFor(c => c.Parameters).ForEach(p => p.SetValidator(new AddCategoryParameterVMValidator()));
    }
}
