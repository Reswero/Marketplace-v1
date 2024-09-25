using FluentValidation;

namespace Marketplace.Users.Application.Staffs.Commands.UpdateStaff;

public class UpdateStaffCommandValidator : AbstractValidator<UpdateStaffCommand>
{
    public UpdateStaffCommandValidator()
    {
        RuleFor(s => s.FirstName).NotEmpty().MinimumLength(2).MaximumLength(20);
        RuleFor(s => s.LastName).NotEmpty().MinimumLength(2).MaximumLength(40);
    }
}
