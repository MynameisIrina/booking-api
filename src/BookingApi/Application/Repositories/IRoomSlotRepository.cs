using BookingApi.Application.Common.Pagination;
using BookingApi.Domain.Entities;

namespace BookingApi.Application.Repositories;

public interface IRoomSlotRepository
{
    public Task<Guid> CreateRoomSlotAsync(RoomSlot roomSlot, CancellationToken cancellationToken);
    public Task<PagedResponse<RoomSlot>> GetAvailableRoomSlotsAsync(PagedRequest request, CancellationToken cancellationToken);
    public Task<RoomSlot?> GetRoomSlotByNameAndDateAsync(string roomName, DateTime slotDate, CancellationToken cancellationToken);
    public Task<RoomSlot?> GetRoomSlotByIdTrackedAsync(Guid roomSlotId, CancellationToken cancellationToken);
    public Task<Guid> UpdateRoomSlotAsync(RoomSlot roomSlot, CancellationToken cancellationToken);
}