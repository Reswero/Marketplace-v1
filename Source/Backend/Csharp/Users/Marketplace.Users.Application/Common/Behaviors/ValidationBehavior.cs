using FluentValidation;
using MediatR;

namespace Marketplace.Users.Application.Common.Behaviors;

internal class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) :
    IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var validationErrors = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var errors = validationErrors.Where(v => !v.IsValid)
            .SelectMany(v => v.Errors)
            .ToList();

        if (errors.Count > 0)
            throw new ValidationException(errors);

        return await next();
    }
}
