using Ardalis.Result;
using BookingApi.Application.Repositories;
using BookingApi.Application.UseCases.Bookings.CreateBooking;
using BookingApi.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Application.UseCases.Bookings.Commands.CreateBooking
{
    public class CreateBookingCommandHandler(IBookingRepository bookingRepository, IRoomSlotRepository roomSlotRepository) : IRequestHandler<CreateBookingCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var roomSlot = await roomSlotRepository.GetRoomSlotByIdAsync(request.RoomSlotId);
                if (roomSlot is null)
                {
                    return Result<Guid>.NotFound($"Room slot with ID {request.RoomSlotId} not found.");
                }
                if(roomSlot.IsBooked)
                {
                    return Result<Guid>.Conflict($"Room slot with ID {request.RoomSlotId} is already booked.");
                }

                var booking = new Booking(request.RoomSlotId, request.UserEmail);
                await bookingRepository.CreateBookingAsync(booking);
                roomSlot.Book(request.UserEmail);
                
                return Result<Guid>.Success(booking.Id);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Result<Guid>.Conflict($"Room slot with ID {request.RoomSlotId} is already booked.");
            }
        }
    }
}