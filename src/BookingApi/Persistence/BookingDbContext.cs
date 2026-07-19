using BookingApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace BookingApi.Persistence
{
    public class BookingDbContext: DbContext
    {
        public DbSet<RoomSlot> RoomSlots { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) {}
    }
}