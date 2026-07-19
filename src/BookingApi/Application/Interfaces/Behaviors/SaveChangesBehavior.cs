using BookingApi.Application.Interfaces.Abstractions;
using BookingApi.Persistence;
using MediatR;

namespace BookingApi.Application.Interfaces.Behaviors
{
    public class SaveChangesBehavior<TRequest, TResponse>(BookingDbContext dbContext)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var result = await next();
            await dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}