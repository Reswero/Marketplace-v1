using FluentValidation;
using Marketplace.Orders.Application.Common.Validators;
using Marketplace.Orders.Application.Common.ViewModels;

namespace Marketplace.Orders.Application.Orders.Queries.GetSellerOrders;

internal class GetSellerOrdersQueryValidator : AbstractValidator<GetSellerOrdersQuery>
{
    public GetSellerOrdersQueryValidator()
    {
        RuleFor(q => q.Pagination).SetValidator(new PaginationVMValidator());
    }
}
