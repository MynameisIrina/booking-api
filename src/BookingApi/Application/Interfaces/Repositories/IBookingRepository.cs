using BookingApi.Domain.Entities;

namespace BookingApi.Application.Repositories
{
    public interface IBookingRepository
    {
        public Task CreateBookingAsync(Booking booking);
    }
}