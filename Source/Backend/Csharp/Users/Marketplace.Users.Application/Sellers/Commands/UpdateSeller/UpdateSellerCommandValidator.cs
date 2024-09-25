using FluentValidation;

namespace Marketplace.Users.Application.Sellers.Commands.UpdateSeller;

public class UpdateSellerCommandValidator : AbstractValidator<UpdateSellerCommand>
{
    public UpdateSellerCommandValidator()
    {
        RuleFor(s => s.FirstName).NotEmpty().MinimumLength(2).MaximumLength(20);
        RuleFor(s => s.LastName).NotEmpty().MinimumLength(2).MaximumLength(40);
        RuleFor(s => s.CompanyName).NotEmpty().MinimumLength(6).MaximumLength(100);
    }
}
