using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Result;
using BookingApi.Application.Interfaces.Abstractions;
using MediatR;

namespace BookingApi.Application.UseCases.Bookings.Commands.DeleteBooking
{
    public sealed record DeleteBookingCommand(Guid Id) : ICommand<Result>;
}