using FluentValidation;

namespace Marketplace.Products.Application.Categories.ViewModels.Validators;

public class AddCategoryParameterVMValidator : AbstractValidator<AddCategoryParamterVM>
{
    public AddCategoryParameterVMValidator()
    {
        RuleFor(p => p.Name).Length(2, 100);
        RuleFor(p => p.Type).IsInEnum();
    }
}
