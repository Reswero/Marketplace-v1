using FluentValidation;

namespace Marketplace.Orders.Application.Orders.Commands.CreateOrder;

internal class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleForEach(o => o.Products).ChildRules(p =>
        {
            p.RuleFor(p => p.Quantity).GreaterThan(0);
        });
    }
}
