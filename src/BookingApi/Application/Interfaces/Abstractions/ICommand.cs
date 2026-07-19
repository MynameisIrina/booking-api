using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace BookingApi.Application.Interfaces.Abstractions
{
    public interface ICommand<TResponse> : IRequest<TResponse> {}
}