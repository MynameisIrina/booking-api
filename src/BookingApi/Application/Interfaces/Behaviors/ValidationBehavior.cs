using FluentValidation;
using MediatR;

namespace BookingApi.Application.Interfaces.Behaviors
{
    /// <summary>
    /// A MediatR pipeline behavior that validates requests using FluentValidation validators.
    /// It is used for both commands and queries that implement the IRequest interface.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="validators"></param>
    /// <param name="logger"></param>
    public class ValidationBehavior<TRequest, TResponse>(
        IEnumerable<IValidator<TRequest>> validators,
        ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Validating request of type {typeof(TRequest).Name}");
            if(validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures = results.SelectMany(r => r.Errors).Where(f => f != null).ToList();
                if(failures.Any())
                {
                    logger.LogWarning("Validation failed for request of type {RequestType}", typeof(TRequest).Name);
                    throw new ValidationException(failures);
                }

                Console.WriteLine($"Validation passed for request of type {typeof(TRequest).Name}");
            }
    
            return await next();
        }
    }
}