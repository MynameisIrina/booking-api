using MediatR;

namespace BookingApi.Application.Interfaces.Abstractions
{
    public interface IQuery<TResponse> : IRequest<TResponse> { }
}