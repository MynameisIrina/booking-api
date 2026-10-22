using Ardalis.Result;
using BookingApi.Application.Repositories;
using MediatR;

namespace BookingApi.Application.UseCases.Bookings.Queries.GetById;

public class GetBookingByIdQueryHandler(IBookingRepository bookingRepository) : IRequestHandler<GetBookingByIdQuery, Result<BookingDto>>
{
    public async Task<Result<BookingDto>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetByIdAsync(request.Id, cancellationToken);
        if (booking is null)
        {
            return Result<BookingDto>.NotFound("Booking was not found.");
        }

        var bookingDto = new BookingDto(booking.RoomSlotId, booking.UserEmail, booking.CreatedAt);
        return Result.Success(bookingDto);
    }
}