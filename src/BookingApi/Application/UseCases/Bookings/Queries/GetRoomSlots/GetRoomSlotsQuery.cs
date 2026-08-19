using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Result;
using BookingApi.Application.Common.Pagination;
using MediatR;

namespace BookingApi.Application.UseCases.Bookings.Queries.GetRoomSlots
{
    public sealed record GetRoomSlotsQuery(PagedRequest PagedRequest) : IRequest<Result<PagedResponse<RoomSlotDto>>>;

    public sealed record RoomSlotDto(Guid Id, string RoomName, DateTime SlotDate);
}