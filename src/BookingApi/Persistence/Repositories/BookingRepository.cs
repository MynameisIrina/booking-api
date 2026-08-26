using BookingApi.Application.Repositories;
using BookingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Persistence.Repositories
{
    public class BookingRepository(BookingDbContext dbContext) : IBookingRepository
    {
        public Task CreateBookingAsync(Booking booking)
        {
            dbContext.Add(booking);
            return Task.CompletedTask;
        }

        public Task Delete(Booking booking)
        {
            dbContext.Remove(booking);
            return Task.CompletedTask;
        }

        public async Task<Booking?> GetByIdAsync(Guid Id)
        {
            return await dbContext.Bookings
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == Id);
        }
    }
}