using FluentValidation;

namespace Marketplace.Users.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().MinimumLength(2).MaximumLength(20);
        RuleFor(c => c.LastName).NotEmpty().MinimumLength(2).MaximumLength(40);
    }
}
