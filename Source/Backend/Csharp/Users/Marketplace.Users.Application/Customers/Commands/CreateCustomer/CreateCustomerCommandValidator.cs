using FluentValidation;

namespace Marketplace.Users.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().MinimumLength(2).MaximumLength(20);
        RuleFor(c => c.LastName).NotEmpty().MinimumLength(2).MaximumLength(40);
    }
}
