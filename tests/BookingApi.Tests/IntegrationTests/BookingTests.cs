using Testcontainers.PostgreSql;
using BookingApi.Persistence;
using Microsoft.EntityFrameworkCore;
using BookingApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using BookingApi.API.Endpoints.RoomSlots;


namespace BookingApi.Tests.IntegrationTests
{
    public class BookingTests : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<Program> _baseFactory;
        private WebApplicationFactory<Program> _factory = default!;
        private HttpClient _client = default!;

        private readonly PostgreSqlContainer _postgresqlContainer = new PostgreSqlBuilder()
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithPassword("testpassword")
            .Build();

        public BookingTests(WebApplicationFactory<Program> baseFactory)
        {
            _baseFactory = baseFactory;
        }

        public async Task InitializeAsync()
        {
            await _postgresqlContainer.StartAsync();

            _factory = _baseFactory.WithWebHostBuilder(builder =>
            {
                builder.UseSetting("ConnectionStrings:DefaultConnection", _postgresqlContainer.GetConnectionString());
            });

            _client = _factory.CreateClient();
        }

        public async Task DisposeAsync()
        {
            await _postgresqlContainer.StopAsync();
            await _factory.DisposeAsync();
        }

        private IServiceScope CreateScope()
        {
            return _factory.Services.CreateScope();
        }

        [Fact]
        public async Task TwoBookingsForSameRoomAndTime_ShouldReturnConflict()
        {
            // Arrange
            Guid roomSlotId;
            using (var scope = CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();

                var roomSlot = new RoomSlot("TestName", DateTime.UtcNow);
                dbContext.RoomSlots.Add(roomSlot);
                await dbContext.SaveChangesAsync();
                roomSlotId = roomSlot.Id;
            }

            // Act
            var requestBody = new
            {
                RoomSlotId = roomSlotId,
                UserEmail = "test@example.com"
            };

            var task1 = _client.PostAsJsonAsync("/bookings/create", requestBody);
            var response1 = await task1;
            var task2 = _client.PostAsJsonAsync("/bookings/create", requestBody);
            var response2 = await task2;

            // Assert
            var successCount = 0;
            var conflictCount = 0;

            if (response1.IsSuccessStatusCode)
                successCount++;

            if (response2.IsSuccessStatusCode)
                successCount++;

            if (response1.StatusCode == System.Net.HttpStatusCode.Conflict)
                conflictCount++;

            if (response2.StatusCode == System.Net.HttpStatusCode.Conflict)
                conflictCount++;

            Assert.Equal(1, successCount);
            Assert.Equal(1, conflictCount);

            using (var scope = CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
                var bookingsCount = await dbContext.Bookings.CountAsync(b => b.RoomSlotId == roomSlotId);
                Assert.Equal(1, bookingsCount);
            }
        }

        [Fact]
        public async Task DeleteBooking_ShouldFreeUpRoomSlot()
        {
            Guid bookingId;
            Guid roomSlotId;
            DateTime slotDate;
            // Arrange
            using (var scope = CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();

                var roomSlot = new RoomSlot("TestName", DateTime.UtcNow);
                dbContext.RoomSlots.Add(roomSlot);
                await dbContext.SaveChangesAsync();

                var booking = new Booking(roomSlot.Id, "test@example.com");
                dbContext.Bookings.Add(booking);
                await dbContext.SaveChangesAsync();

                bookingId = booking.Id;
                roomSlotId = roomSlot.Id;
                slotDate = roomSlot.SlotDate;
            }

            // Act
            var requestBody = new
            {
                RoomSlotId = roomSlotId,
                UserEmail = "test@example.com"
            };

            await _client.PostAsJsonAsync("/bookings/create", requestBody);
            await _client.DeleteAsync("/bookings/" + bookingId);

            var task3 = _client.GetAsync("/room-slots/available?page=1&pageSize=10");
            var availableSlotsResponse = await task3;
            var bodyResponse = await availableSlotsResponse.Content.ReadFromJsonAsync<GetRoomSlotsResponse>();

            // Assert
            Assert.Contains(bodyResponse!.RoomSlots.Data, s => s.Id == roomSlotId);
        }
    }
}