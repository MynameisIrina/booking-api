using BookingApi.Persistence;
using Microsoft.EntityFrameworkCore;
using BookingApi.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using BookingApi.API.Endpoints.RoomSlots;
using BookingApi.API.Endpoints.Bookings;


namespace BookingApi.Tests.IntegrationTests;

public class BookingTests : IClassFixture<DatabaseFixture>, IAsyncLifetime
{
    private readonly DatabaseFixture _sharedContext;
    private HttpClient _client = default!;

    public BookingTests(DatabaseFixture databaseFixture)
    {
        _sharedContext = databaseFixture;
    }

    public async Task InitializeAsync()
    {
        using var scope = CreateScope();
        await scope.ServiceProvider.GetRequiredService<BookingDbContext>().Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Bookings\", \"RoomSlots\";");
        _client = _sharedContext.CreateClient();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    private IServiceScope CreateScope()
    {
        return _sharedContext.Services.CreateScope();
    }

    [Fact]
    public async Task TwoBookingsForSameRoomAndTime_ShouldReturnConflictAndOnlyFirstBookingShouldBeCreated()
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

        var response1 = await _client.PostAsJsonAsync("/bookings/create", requestBody);
        var response2 = await _client.PostAsJsonAsync("/bookings/create", requestBody);

        // Assert
        Assert.True(response1.IsSuccessStatusCode);
        Assert.Equal(System.Net.HttpStatusCode.Conflict, response2.StatusCode);

        using (var scope = CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
            var bookingsCount = await dbContext.Bookings.CountAsync(b => b.RoomSlotId == roomSlotId);
            Assert.Equal(1, bookingsCount);
        }
    }

    [Fact]
    public async Task CreateTwoBookingAtTheSameTime_ShouldReturnConflictForSecondBooking()
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
        var requestBody1 = new
        {
            RoomSlotId = roomSlotId,
            UserEmail = "test@example.com"
        };

        var task1 = _client.PostAsJsonAsync("/bookings/create", requestBody1);
        var task2 = _client.PostAsJsonAsync("/bookings/create", requestBody1);
        await Task.WhenAll(task1, task2);

        var response1 = await task1;
        var response2 = await task2;

        Assert.True(response1.IsSuccessStatusCode || response2.IsSuccessStatusCode);
        Assert.True(response1.StatusCode == System.Net.HttpStatusCode.Conflict || response2.StatusCode == System.Net.HttpStatusCode.Conflict);
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
        Guid roomSlotId;
        // Arrange
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

        var bookingResponse = await _client.PostAsJsonAsync("/bookings/create", requestBody);
        Assert.True(bookingResponse.IsSuccessStatusCode);
        var bookingId = (await bookingResponse.Content.ReadFromJsonAsync<CreateBookingResponse>())!.BookingId;

        var deleteResponse = await _client.DeleteAsync("/bookings/" + bookingId);
        Assert.True(deleteResponse.IsSuccessStatusCode);

        var availableSlotsResponse = await _client.GetAsync("/room-slots/available?page=1&pageSize=10");
        Assert.True(availableSlotsResponse.IsSuccessStatusCode);
        var bodyResponse = await availableSlotsResponse.Content.ReadFromJsonAsync<GetRoomSlotsResponse>();

        Assert.Contains(bodyResponse!.RoomSlots.Data, s => s.Id == roomSlotId);
    }
}