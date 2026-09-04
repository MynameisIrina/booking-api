using BookingApi.Application.Common.Pagination;
using BookingApi.Domain.Entities;

namespace BookingApi.Application.Repositories
{
    public interface IRoomSlotRepository
    {
        public Task<Guid> CreateRoomSlotAsync(RoomSlot roomSlot);
        public Task<PagedResponse<RoomSlot>> GetAvailableRoomSlotsAsync(PagedRequest request);
        public Task<RoomSlot?> GetRoomSlotByNameAndDateAsync(string roomName, DateTime slotDate);
        public Task<RoomSlot?> GetRoomSlotByIdTrackedAsync(Guid roomSlotId);
        public Task UpdateRoomSlotAsync(RoomSlot roomSlot);
        public Task SaveChangesAsync();
    }
}