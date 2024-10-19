using FluentValidation;

namespace Marketplace.Products.Application.Products.Commands.UpdateProduct;

internal class UpdateProductCommandValidator : AbstractValidator<UpdateProductWithIdCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().Length(3, 200);
        RuleFor(p => p.Description).NotEmpty().Length(3, 1000);
        RuleFor(p => p.Price).GreaterThan(0);
    }
}
