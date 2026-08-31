using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Result;
using BookingApi.Application.Common.Pagination;
using BookingApi.Application.Interfaces.Repositories;
using MediatR;

namespace BookingApi.Application.UseCases.Bookings.Queries.GetRoomSlots
{
    public class GetRoomSlotsQueryHandler(IRoomSlotRepository roomSlotRepository) : IRequestHandler<GetRoomSlotsQuery, Result<PagedResponse<RoomSlotDto>>>
    {
        public async Task<Result<PagedResponse<RoomSlotDto>>> Handle(GetRoomSlotsQuery request, CancellationToken cancellationToken)
        {
            var roomSlots = await roomSlotRepository.GetAvailableRoomSlotsAsync(request.PagedRequest);

            var roomSlotsDtos = roomSlots.Data.Select(rs => new RoomSlotDto(rs.Id, rs.RoomName, rs.SlotDate)).ToList();
            var pagedResponse = new PagedResponse<RoomSlotDto>
            {
                Data = roomSlotsDtos,
                Page = roomSlots.Page,
                PageSize = roomSlots.PageSize,
                TotalCount = roomSlots.TotalCount
            };
            return Result<PagedResponse<RoomSlotDto>>.Success(pagedResponse);
        }
    }
}