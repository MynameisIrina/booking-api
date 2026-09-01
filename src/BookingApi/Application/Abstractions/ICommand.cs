using MediatR;

namespace BookingApi.Application.Abstractions
{
    public interface ICommand<TResponse> : IRequest<TResponse> { }
}