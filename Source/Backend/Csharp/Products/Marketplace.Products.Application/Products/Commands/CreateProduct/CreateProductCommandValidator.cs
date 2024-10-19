using FluentValidation;

namespace Marketplace.Products.Application.Products.Commands.CreateProduct;

internal class CreateProductCommandValidator : AbstractValidator<CreateProductWithSellerIdCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().Length(3, 200);
        RuleFor(p => p.Description).NotEmpty().Length(3, 1000);
        RuleFor(p => p.Price).GreaterThan(0);
    }
}
