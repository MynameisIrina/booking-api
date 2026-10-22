using BookingApi.Domain.Entities;

namespace BookingApi.Application.Repositories;

public interface IBookingRepository
{
    public Task<Booking?> GetByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    public Task CreateBookingAsync(Booking booking, CancellationToken cancellationToken = default);
    public Task Delete(Booking booking, CancellationToken cancellationToken = default);
}