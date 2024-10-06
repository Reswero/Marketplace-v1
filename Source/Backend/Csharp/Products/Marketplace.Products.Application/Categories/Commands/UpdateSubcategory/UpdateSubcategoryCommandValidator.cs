using FluentValidation;

namespace Marketplace.Products.Application.Categories.Commands.UpdateSubcategory;

public class UpdateSubcategoryCommandValidator : AbstractValidator<UpdateSubcategoryWithCategoryIdCommand>
{
    public UpdateSubcategoryCommandValidator()
    {
        RuleFor(s => s.Name).MinimumLength(2).MaximumLength(100);
    }
}
