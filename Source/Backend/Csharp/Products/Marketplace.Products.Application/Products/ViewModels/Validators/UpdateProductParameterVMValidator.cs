using FluentValidation;
using Marketplace.Products.Domain.Parameters;
using System.Reflection.Metadata;

namespace Marketplace.Products.Application.Products.ViewModels.Validators;

internal class UpdateProductParameterVMValidator : AbstractValidator<UpdateProductParameterVM>
{
    public UpdateProductParameterVMValidator(CategoryParameter parameter)
    {
        RuleFor(p => p.Value).Length(1, 50);

        if (parameter.Type == ParameterType.Number)
        {
            RuleFor(p => p.Value).Matches("^[0-9]*$");
        }
    }
}
