using BookingApi.Application.Common.Pagination;
using BookingApi.Domain.Entities;

namespace BookingApi.Application.Repositories;

public interface IRoomSlotRepository
{
    public Task<Guid> CreateRoomSlotAsync(RoomSlot roomSlot, CancellationToken cancellationToken = default);
    public Task<PagedResponse<RoomSlot>> GetAvailableRoomSlotsAsync(PagedRequest request, CancellationToken cancellationToken = default);
    public Task<RoomSlot?> GetRoomSlotByNameAndDateAsync(string roomName, DateTime slotDate, CancellationToken cancellationToken = default);
    public Task<RoomSlot?> GetRoomSlotByIdTrackedAsync(Guid roomSlotId, CancellationToken cancellationToken = default);
    public Task UpdateRoomSlotAsync(RoomSlot roomSlot, CancellationToken cancellationToken = default);
}