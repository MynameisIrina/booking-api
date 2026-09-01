using Ardalis.Result;
using BookingApi.Application.Abstractions;

namespace BookingApi.Application.UseCases.Bookings.Commands.DeleteBooking
{
    public sealed record DeleteBookingCommand(Guid Id) : ICommand<Result>;
}