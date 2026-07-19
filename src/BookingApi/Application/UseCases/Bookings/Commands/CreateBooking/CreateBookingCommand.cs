using Ardalis.Result;
using BookingApi.Application.Interfaces.Abstractions;

namespace BookingApi.Application.UseCases.Bookings.CreateBooking
{
    public sealed record CreateBookingCommand(Guid RoomSlotId, string UserEmail) : ICommand<Result<Guid>>;
}