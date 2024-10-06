using FluentValidation;

namespace Marketplace.Products.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(c => c.Name).MinimumLength(2).MaximumLength(100);
    }
}
