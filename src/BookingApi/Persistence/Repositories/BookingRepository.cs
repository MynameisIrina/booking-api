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
    }
}