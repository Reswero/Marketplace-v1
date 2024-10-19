using FluentValidation;
using Marketplace.Products.Application.Categories.ViewModels.Validators;

namespace Marketplace.Products.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Name).Length(2, 100);
        RuleFor(c => c.Parameters).ForEach(p => p.SetValidator(new UpdateCategoryParameterVMValidator()));
    }
}
