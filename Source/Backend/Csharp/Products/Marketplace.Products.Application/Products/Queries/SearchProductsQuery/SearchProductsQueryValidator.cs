using FluentValidation;

namespace Marketplace.Products.Application.Products.Queries.SearchProductsQuery;

internal class SearchProductsQueryValidator : AbstractValidator<SearchProductsQuery>
{
    public SearchProductsQueryValidator()
    {
        RuleFor(q => q.SortType).IsInEnum();

        When(q => q.Pagination is not null, () =>
        {
            RuleFor(q => q.Pagination!.Offset).GreaterThanOrEqualTo(0);
            RuleFor(q => q.Pagination!.Limit).GreaterThanOrEqualTo(0)
                .LessThan(200);
        });

        When(q => q.PriceFilter is not null, () =>
        {
            RuleFor(q => q.PriceFilter!.From).GreaterThanOrEqualTo(0);

            When(q => q.PriceFilter!.To is not null, () =>
            {
                RuleFor(q => q.PriceFilter!.To!).GreaterThanOrEqualTo(0);
            });
        });
    }
}
