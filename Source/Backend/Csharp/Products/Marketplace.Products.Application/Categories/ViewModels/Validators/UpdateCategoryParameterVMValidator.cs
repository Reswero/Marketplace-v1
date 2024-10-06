using FluentValidation;

namespace Marketplace.Products.Application.Categories.ViewModels.Validators;

public class UpdateCategoryParameterVMValidator : AbstractValidator<UpdateCategoryParameterVM>
{
    public UpdateCategoryParameterVMValidator()
    {
        RuleFor(p => p.Name).MinimumLength(2).MaximumLength(100);
        RuleFor(p => p.Type).IsInEnum();
    }
}
