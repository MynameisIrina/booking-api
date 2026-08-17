using BookingApi.Domain.Entities;

namespace BookingApi.Application.Repositories
{
    public interface IBookingRepository
    {
        public Task<Booking?> GetBookingByIdAsync(Guid Id);
        public Task CreateBookingAsync(Booking booking);
    }
}