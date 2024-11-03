using FluentValidation;
using Marketplace.Products.Application.Common;

namespace Marketplace.Products.Application.Products.Commands.UploadImages;

internal class UploadImagesCommandValidator : AbstractValidator<UploadImagesCommand>
{
    public UploadImagesCommandValidator()
    {
        RuleFor(c => c.Images).NotEmpty();
        RuleFor(c => c.Images).ForEach(file =>
        {
            file.ChildRules(validator =>
            {
                validator.RuleFor(f => f.Stream.Length).LessThanOrEqualTo(ApplicationConsts.MaxImageSize);
            });
        });
    }
}
