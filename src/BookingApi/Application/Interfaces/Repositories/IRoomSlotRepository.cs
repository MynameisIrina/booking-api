using BookingApi.Application.Common.Pagination;
using BookingApi.Domain.Entities;

namespace BookingApi.Application.Repositories
{
    public interface IRoomSlotRepository 
    {
        public Task<PagedResponse<RoomSlot>> GetAvailableRoomSlotsAsync(PagedRequest request);
        public Task<RoomSlot?> GetRoomSlotByIdAsync(Guid roomSlotId);
        public Task UpdateRoomSlotAsync(RoomSlot roomSlot);
        public Task SaveChangesAsync();
    }
}