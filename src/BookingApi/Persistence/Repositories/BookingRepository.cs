using BookingApi.Application.Repositories;
using BookingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Persistence.Repositories;

public class BookingRepository(BookingDbContext dbContext) : IBookingRepository
{
    public Task CreateBookingAsync(Booking booking, CancellationToken cancellationToken)
    {
        dbContext.Add(booking);
        return Task.CompletedTask;
    }

    public Task Delete(Booking booking, CancellationToken cancellationToken)
    {
        dbContext.Remove(booking);
        return Task.CompletedTask;
    }

    public async Task<Booking?> GetByIdAsync(Guid Id, CancellationToken cancellationToken)
    {
        return await dbContext.Bookings
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == Id, cancellationToken);
    }
}