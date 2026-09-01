using MediatR;

namespace BookingApi.Application.Interfaces.Abstractions
{
    public interface ICommand<TResponse> : IRequest<TResponse> { }
}