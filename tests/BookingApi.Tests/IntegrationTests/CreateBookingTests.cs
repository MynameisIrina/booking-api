using Testcontainers.PostgreSql;
using BookingApi.Persistence;
using Microsoft.EntityFrameworkCore;
using BookingApi.Domain.Entities;


namespace BookingApi.Tests.IntegrationTests
{
    public class CreateBookingTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgresqlContainer = new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .Build();

        private BookingDbContext _dbContext = default!;

        public async Task InitializeAsync()
        {
            await _postgresqlContainer.StartAsync();
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                 .UseNpgsql(_postgresqlContainer.GetConnectionString())
                 .Options;

            _dbContext = new BookingDbContext(options);
            await _dbContext.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await _postgresqlContainer.StopAsync();
            await _dbContext.DisposeAsync();
        }

        [Fact]
        public async Task TwoSimultaneousBookings_OnlyOneSucceeds()
        {
            // Arrange
            var roomSlot = new RoomSlot("TestName", DateTime.UtcNow);
            _dbContext.RoomSlots.Add(roomSlot);
            await _dbContext.SaveChangesAsync();

            var task1 = BookSlotAsync(roomSlot.Id, "user1@example.com");
            var task2 = BookSlotAsync(roomSlot.Id, "user2@example.com");
            var results = await Task.WhenAll(task1, task2);

            Assert.Equal(1, results.Count(r => r == true));
            Assert.Equal(1, results.Count(r => r == false));
        }

        private async Task<bool> BookSlotAsync(Guid roomSlotId, string email)
        {
            var options = new DbContextOptionsBuilder<BookingDbContext>()
                .UseNpgsql(_postgresqlContainer.GetConnectionString())
                .Options;

            using var context = new BookingDbContext(options);

            try
            {
                var roomSlot = await context.RoomSlots.FindAsync(roomSlotId);
                if (roomSlot == null || roomSlot.IsBooked)
                {
                    return false;
                }

                roomSlot.Book(email);
                var booking = new Booking(roomSlotId, email);
                context.Bookings.Add(booking);
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }
    }
}