using FluentValidation;

namespace Marketplace.Users.Application.Staffs.Commands.CreateStaff;

public class CreateStaffCommandValidator : AbstractValidator<CreateStaffCommand>
{
    public CreateStaffCommandValidator()
    {
        RuleFor(s => s.FirstName).NotEmpty().MinimumLength(2).MaximumLength(20);
        RuleFor(s => s.LastName).NotEmpty().MinimumLength(2).MaximumLength(40);
    }
}
