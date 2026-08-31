using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Result;
using BookingApi.Application.Interfaces.Repositories;
using MediatR;

namespace BookingApi.Application.UseCases.Bookings.Commands.GetById
{
    public class GetBookingByIdCommandHandler(IBookingRepository bookingRepository) : IRequestHandler<GetBookingByIdCommand, Result<BookingDto>>
    {
        public async Task<Result<BookingDto>> Handle(GetBookingByIdCommand request, CancellationToken cancellationToken)
        {
            var booking = await bookingRepository.GetByIdAsync(request.Id);
            if(booking is null)
            {
                return Result<BookingDto>.NotFound("Booking was not found.");
            }

            var bookingDto = new BookingDto(booking.Id, booking.UserEmail, booking.CreatedAt);
            return Result.Success(bookingDto);
        }
    }
}