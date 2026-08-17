using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Result;
using BookingApi.Application.Repositories;
using FastEndpoints;
using MediatR;

namespace BookingApi.Application.UseCases.Bookings.Queries.GetRoomSlots
{
    public class GetRoomSlotsQueryHandler(IRoomSlotRepository roomSlotRepository) : IRequestHandler<GetRoomSlotsQuery, Result<List<RoomSlotDto>>>
    {
        public async Task<Result<List<RoomSlotDto>>> Handle(GetRoomSlotsQuery request, CancellationToken cancellationToken)
        {
            var roomSlots = await roomSlotRepository.GetAvailableRoomSlotsAsync();
            if(!roomSlots.Any())
            {
                return Result<List<RoomSlotDto>>.NotFound("No available room slots found.");
            }
            var roomSlotsDtos = roomSlots.Select(rs => new RoomSlotDto(rs.Id, rs.RoomName, rs.SlotDate)).ToList();
            return Result<List<RoomSlotDto>>.Success(roomSlotsDtos);
        }
    }
}