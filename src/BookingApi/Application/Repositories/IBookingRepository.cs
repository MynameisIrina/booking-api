using BookingApi.Domain.Entities;

namespace BookingApi.Application.Repositories
{
    public interface IBookingRepository
    {
        public Task<Booking?> GetByIdAsync(Guid Id);
        public Task CreateBookingAsync(Booking booking);
        public Task Delete(Booking booking);
    }
}