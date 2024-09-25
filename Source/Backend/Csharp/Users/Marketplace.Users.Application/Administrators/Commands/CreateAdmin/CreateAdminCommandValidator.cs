using FluentValidation;

namespace Marketplace.Users.Application.Administrators.Commands.CreateAdmin;

public class CreateAdminCommandValidator : AbstractValidator<CreateAdminCommand>
{
    public CreateAdminCommandValidator()
    {
        RuleFor(a => a.FirstName).NotEmpty().MinimumLength(2).MaximumLength(20);
        RuleFor(a => a.LastName).NotEmpty().MinimumLength(2).MaximumLength(40);
    }
}
