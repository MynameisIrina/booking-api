using Ardalis.Result;
using BookingApi.Application.Repositories;
using MediatR;

namespace BookingApi.Application.UseCases.Bookings.Commands.DeleteBooking
{
    public class DeleteBookingCommandHandler(IBookingRepository bookingRepository, IRoomSlotRepository roomSlotRepository) : IRequestHandler<DeleteBookingCommand, Result>
    {
        public async Task<Result> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await bookingRepository.GetByIdAsync(request.Id);
            if (booking is null)
                return Result.NotFound();

            var roomSlot = await roomSlotRepository.GetRoomSlotByIdTrackedAsync(booking.RoomSlotId);
            if (roomSlot is null)
                return Result.NotFound($"Room slot with ID {booking.RoomSlotId} not found.");

            var ensureDeleteResult = booking.EnsureCanBeDeleted();
            if (!ensureDeleteResult.IsSuccess)
                return ensureDeleteResult;

            var freeUpResult = roomSlot.FreeUp();
            if (!freeUpResult.IsSuccess)
                return freeUpResult;

            await bookingRepository.Delete(booking);
            return Result.Success();
        }
    }
}