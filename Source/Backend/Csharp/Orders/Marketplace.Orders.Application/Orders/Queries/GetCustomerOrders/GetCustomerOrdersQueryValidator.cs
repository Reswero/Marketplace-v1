using FluentValidation;
using Marketplace.Orders.Application.Common.Validators;

namespace Marketplace.Orders.Application.Orders.Queries.GetCustomerOrders;

internal class GetCustomerOrdersQueryValidator : AbstractValidator<GetCustomerOrdersQuery>
{
    public GetCustomerOrdersQueryValidator()
    {
        RuleFor(q => q.Pagination).SetValidator(new PaginationVMValidator());
    }
}
