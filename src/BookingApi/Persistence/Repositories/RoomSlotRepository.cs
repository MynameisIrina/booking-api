using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingApi.Application.Common.Pagination;
using BookingApi.Application.Repositories;
using BookingApi.Domain.Entities;
using BookingApi.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Persistence.Repositories
{
    public class RoomSlotRepository(BookingDbContext dbContext) : IRoomSlotRepository
    {
        public Task<RoomSlot?> GetRoomSlotByIdAsync(Guid roomSlotId)
        {
            return dbContext.RoomSlots.AsNoTracking().FirstOrDefaultAsync(rs => rs.Id == roomSlotId); 
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

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}