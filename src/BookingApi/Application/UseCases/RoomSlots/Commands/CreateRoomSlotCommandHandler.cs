using Ardalis.Result;
using BookingApi.Application.Repositories;
using BookingApi.Domain.Entities;
using MediatR;

namespace BookingApi.Application.UseCases.RoomSlots.Commands
{
    public class CreateRoomSlotCommandHandler(IRoomSlotRepository roomSlotRepository) : IRequestHandler<CreateRoomSlotCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(CreateRoomSlotCommand request, CancellationToken cancellationToken)
        {
            var existingRoomSlot = await roomSlotRepository.GetRoomSlotByNameAndDateAsync(request.RoomName, request.SlotDate);
            if (existingRoomSlot is not null)
            {
                return Result<Guid>.Conflict($"Room slot for {request.RoomName} on {request.SlotDate} already exists.");
            }

            var roomSlot = new RoomSlot(request.RoomName, request.SlotDate);
            var roomSlotId = await roomSlotRepository.CreateRoomSlotAsync(roomSlot);
            return Result<Guid>.Success(roomSlotId);
        }
    }
}