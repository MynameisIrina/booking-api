using Ardalis.Result;
using BookingApi.Application.Abstractions;

namespace BookingApi.Application.UseCases.Bookings.Commands.GetById
{
    public sealed record GetBookingByIdCommand(Guid Id) : IQuery<Result<BookingDto>>;

    public sealed record BookingDto(Guid RoomSlotId, string UserEmail, DateTimeOffset CreatedAt);
}