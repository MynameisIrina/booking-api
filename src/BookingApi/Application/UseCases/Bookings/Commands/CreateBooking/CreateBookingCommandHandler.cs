using Ardalis.Result;
using BookingApi.Application.Repositories;
using BookingApi.Domain.Entities;
using MediatR;

namespace BookingApi.Application.UseCases.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler(IBookingRepository bookingRepository, IRoomSlotRepository roomSlotRepository, ILogger<CreateBookingCommandHandler> logger) : IRequestHandler<CreateBookingCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var roomSlot = await roomSlotRepository.GetRoomSlotByIdTrackedAsync(request.RoomSlotId);
            if (roomSlot is null)
            {
                return Result<Guid>.NotFound($"Room slot with ID {request.RoomSlotId} not found.");
            }

            var booking = new Booking(request.RoomSlotId, request.UserEmail);
            await bookingRepository.CreateBookingAsync(booking);
            var bookResult = roomSlot.Book(request.UserEmail);
            if (!bookResult.IsSuccess)
            {
                return Result<Guid>.Conflict(bookResult.Errors.ToArray());
            }

            logger.LogInformation("User booking created for slot {RoomSlotId}", request.RoomSlotId);

            return Result<Guid>.Success(booking.Id);
        }
    }
}