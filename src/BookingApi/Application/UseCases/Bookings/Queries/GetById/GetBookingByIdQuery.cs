using Ardalis.Result;
using BookingApi.Application.Abstractions;

namespace BookingApi.Application.UseCases.Bookings.Queries.GetById;

public sealed record GetBookingByIdQuery(Guid Id) : IQuery<Result<BookingDto>>;

public sealed record BookingDto(Guid RoomSlotId, string UserEmail, DateTimeOffset CreatedAt);