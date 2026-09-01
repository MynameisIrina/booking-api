using Ardalis.Result;
using BookingApi.Application.Interfaces.Abstractions;

namespace BookingApi.Application.UseCases.Bookings.Commands.DeleteBooking
{
    public sealed record DeleteBookingCommand(Guid Id) : ICommand<Result>;
}