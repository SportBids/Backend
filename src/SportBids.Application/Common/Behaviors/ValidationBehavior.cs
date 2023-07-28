using FluentResults;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using SportBids.Application.Common.Errors;

namespace SportBids.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ResultBase, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationErrors = _validators
            .Select(validator => validator.Validate(context))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new { failure.PropertyName, failure.ErrorMessage })
            .ToArray();
        
        if (!validationErrors.Any())
        {
            return await next();
        }
        
        var result = new TResponse();
        var error = new ValidationError();
        foreach (var validationError in validationErrors)
        {
            error.WithMetadata(validationError.PropertyName, validationError.ErrorMessage);
        }
        
        result.Reasons.Add(error);
        
        return result;
    }
}
