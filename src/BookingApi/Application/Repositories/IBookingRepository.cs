using BookingApi.Domain.Entities;

namespace BookingApi.Application.Repositories;

public interface IBookingRepository
{
    public Task<Booking?> GetByIdAsync(Guid Id, CancellationToken cancellationToken);
    public Task CreateBookingAsync(Booking booking, CancellationToken cancellationToken);
    public Task Delete(Booking booking, CancellationToken cancellationToken);
}