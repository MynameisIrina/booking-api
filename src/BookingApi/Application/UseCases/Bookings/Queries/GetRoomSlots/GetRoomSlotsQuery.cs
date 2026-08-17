using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;

namespace BookingApi.Application.UseCases.Bookings.Queries.GetRoomSlots
{
    public sealed record GetRoomSlotsQuery : IRequest<Result<List<RoomSlotDto>>>;

    public sealed record RoomSlotDto(Guid Id, string RoomName, DateTime SlotDate);
}