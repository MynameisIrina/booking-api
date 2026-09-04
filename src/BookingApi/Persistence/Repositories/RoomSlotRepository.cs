using BookingApi.Application.Common.Pagination;
using BookingApi.Application.Repositories;
using BookingApi.Domain.Entities;
using BookingApi.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Persistence.Repositories
{
    public class RoomSlotRepository(BookingDbContext dbContext) : IRoomSlotRepository
    {
        public Task<RoomSlot?> GetRoomSlotByIdTrackedAsync(Guid roomSlotId)
        {
            return dbContext.RoomSlots.FirstOrDefaultAsync(rs => rs.Id == roomSlotId);
        }

        public async Task UpdateRoomSlotAsync(RoomSlot roomSlot)
        {
            dbContext.RoomSlots.Update(roomSlot);
        }

        public async Task<PagedResponse<RoomSlot>> GetAvailableRoomSlotsAsync(PagedRequest request)
        {
            return await dbContext.RoomSlots
                .AsNoTracking()
                .Where(rs => !rs.IsBooked)
                .ToPageResponseAsync(request.Page, request.PageSize);
        }

        public async Task<Guid> CreateRoomSlotAsync(RoomSlot roomSlot)
        {
            dbContext.RoomSlots.Add(roomSlot);
            await dbContext.SaveChangesAsync();
            return roomSlot.Id;
        }

        public async Task<RoomSlot?> GetRoomSlotByNameAndDateAsync(string roomName, DateTime slotDate)
        {
            return await dbContext.RoomSlots.FirstOrDefaultAsync(rs => rs.RoomName == roomName && rs.SlotDate == slotDate);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}