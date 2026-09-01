using MediatR;

namespace BookingApi.Application.Abstractions
{
    public interface IQuery<TResponse> : IRequest<TResponse> { }
}