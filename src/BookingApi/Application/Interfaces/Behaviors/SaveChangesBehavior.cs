using BookingApi.Application.Interfaces.Abstractions;
using BookingApi.Persistence;
using MediatR;

namespace BookingApi.Application.Interfaces.Behaviors
{
    /// <summary>
    /// A MediatR pipeline behavior that saves changes to the database after a command is handled.
    /// It is used only for commands that implement the ICommand interface. 
    /// IQuery requests are not affected by this behavior in order to avoid unnecessary database writes for read-only operations.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request</typeparam>
    /// <typeparam name="TResponse">The type of the response</typeparam>
    public class SaveChangesBehavior<TRequest, TResponse>(
        BookingDbContext dbContext,
        ILogger<SaveChangesBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            // Save changes to the database after the command has been handled
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Changes saved to the database for request of type {RequestType}", typeof(TRequest).Name);

            return response;
            
        }
    }
}