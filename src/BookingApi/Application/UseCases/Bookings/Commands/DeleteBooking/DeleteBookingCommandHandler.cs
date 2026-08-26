using Ardalis.Result;
using BookingApi.Application.Repositories;
using MediatR;

namespace BookingApi.Application.UseCases.Bookings.Commands.DeleteBooking
{
    public class DeleteBookingCommandHandler(IBookingRepository bookingRepository) : IRequestHandler<DeleteBookingCommand, Result>
    {
        public async Task<Result> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await bookingRepository.GetByIdAsync(request.Id);
            if (booking is null)
            {
                return Result.NotFound();
            }

            var result = booking.DeleteBooking();
            if (!result.IsSuccess)
            {
                return result;
            }

            await bookingRepository.Delete(booking);
            return Result.Success();
        }
    }
}