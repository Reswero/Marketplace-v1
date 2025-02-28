using FluentValidation;
using Marketplace.Orders.Application.Common.ViewModels;

namespace Marketplace.Orders.Application.Common.Validators;

internal class PaginationVMValidator : AbstractValidator<PaginationVM>
{
    public PaginationVMValidator()
    {
        RuleFor(p => p.Offset).GreaterThanOrEqualTo(0);
        RuleFor(p => p.Limit).GreaterThanOrEqualTo(0);
    }
}
