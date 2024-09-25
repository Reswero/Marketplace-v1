using FluentValidation;

namespace Marketplace.Users.Application.Sellers.Commands.CreateSeller;

public class CreateSellerCommandValidator : AbstractValidator<CreateSellerCommand>
{
    public CreateSellerCommandValidator()
    {
        RuleFor(s => s.FirstName).NotEmpty().MinimumLength(2).MaximumLength(20);
        RuleFor(s => s.LastName).NotEmpty().MinimumLength(2).MaximumLength(40);
        RuleFor(s => s.CompanyName).NotEmpty().MinimumLength(6).MaximumLength(100);
    }
}
