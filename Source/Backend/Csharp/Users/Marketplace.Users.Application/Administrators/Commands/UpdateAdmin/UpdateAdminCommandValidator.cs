using FluentValidation;

namespace Marketplace.Users.Application.Administrators.Commands.UpdateAdmin;

public class UpdateAdminCommandValidator : AbstractValidator<UpdateAdminCommand>
{
    public UpdateAdminCommandValidator()
    {
        RuleFor(a => a.FirstName).NotEmpty().MinimumLength(2).MaximumLength(20);
        RuleFor(a => a.LastName).NotEmpty().MinimumLength(2).MaximumLength(40);
    }
}
