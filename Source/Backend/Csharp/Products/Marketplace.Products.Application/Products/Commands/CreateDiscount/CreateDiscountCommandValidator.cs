using FluentValidation;

namespace Marketplace.Products.Application.Products.Commands.CreateDiscount;

internal class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountWithProductIdCommand>
{
    public CreateDiscountCommandValidator()
    {
        RuleFor(d => d.Size).GreaterThan(0);
        RuleFor(d => d.ValidUntil).GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
    }
}
