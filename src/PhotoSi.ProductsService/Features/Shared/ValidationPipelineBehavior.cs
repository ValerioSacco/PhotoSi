using FluentValidation;
using MediatR;

namespace PhotoSi.ProductsService.Features.Shared
{
    public class ValidationPipelineBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);

            var validationFailures = await Task.WhenAll(
                _validators.Select(validator => validator.ValidateAsync(context)));

            var errors = validationFailures
                .Where(validationResult => !validationResult.IsValid)
                .SelectMany(validationResult => validationResult.Errors)
                .Select(validationFailure => new ValidationError(
                    validationFailure.PropertyName,
                    validationFailure.ErrorMessage))
                .ToList();

            if (errors.Any())
            {
                throw new ValidationException(errors);
            }

            var response = await next();

            return response;
        }
    }

    public class ValidationException(IEnumerable<ValidationError> Errors)
        : Exception("Validation failed")
    {
        public IEnumerable<ValidationError> Errors { get; } = Errors;
    }

    public record ValidationError(string propertyName, string errorMessage);

}
