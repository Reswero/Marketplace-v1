using FluentValidation;
using Marketplace.Products.Domain.Parameters;

namespace Marketplace.Products.Application.Products.ViewModels.Validators;

public class AddProductParameterVMValidator : AbstractValidator<AddProductParameterVM>
{
    public AddProductParameterVMValidator(CategoryParameter parameter)
    {
        RuleFor(p => p.Value).Length(1, 50);

        if (parameter.Type == ParameterType.Number)
        {
            RuleFor(p => p.Value).Matches("^[0-9]*$");
        }
    }
}
