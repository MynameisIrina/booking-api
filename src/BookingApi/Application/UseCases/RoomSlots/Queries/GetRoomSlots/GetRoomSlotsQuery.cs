using Ardalis.Result;
using BookingApi.Application.Abstractions;
using BookingApi.Application.Common.Pagination;

namespace BookingApi.Application.UseCases.Bookings.Queries.GetRoomSlots
{
    public sealed record GetRoomSlotsQuery(PagedRequest PagedRequest) : IQuery<Result<PagedResponse<RoomSlotDto>>>;

    public sealed record RoomSlotDto(Guid Id, string RoomName, DateTime SlotDate);
}