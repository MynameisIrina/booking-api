using BookingApi.Domain.Entities;

namespace BookingApi.Application.Repositories
{
    public interface IRoomSlotRepository 
    {
        public Task<IReadOnlyList<RoomSlot>> GetAvailableRoomSlotsAsync();
        public Task<RoomSlot?> GetRoomSlotByIdAsync(Guid roomSlotId);
        public Task UpdateRoomSlotAsync(RoomSlot roomSlot);
        public Task SaveChangesAsync();
    }
}