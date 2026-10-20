using System.Net.Http.Json;
using BookingApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApi.Tests.IntegrationTests;

public class RoomSlotTests(DatabaseFixture databaseFixture) : IClassFixture<DatabaseFixture>, IAsyncLifetime
{
    private readonly DatabaseFixture sharedContext = databaseFixture;
    private HttpClient client = default!;

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task InitializeAsync()
    {
        using var scope = CreateScope();
        await scope.ServiceProvider.GetRequiredService<BookingDbContext>().Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Bookings\", \"RoomSlots\";");
        client = sharedContext.CreateClient();
    }

    private IServiceScope CreateScope() => sharedContext.Services.CreateScope();

    [Fact]
    public async Task TwoConcurrentCreateCommandsWithSameNameAndDate_ShouldReturnConflict()
    {
        using (var scope = CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
        }

        var request = new {RoomName = "Test", SlotDate = DateTime.UtcNow, UserEmail = "test@example.com"};

        var task1 = client.PostAsJsonAsync("/room-slots", request);
        var task2 = client.PostAsJsonAsync("/room-slots", request);

        await Task.WhenAll(task1, task2);

        var response1 = await task1;
        var response2 = await task2;

        Assert.True(response1.IsSuccessStatusCode || response2.IsSuccessStatusCode);
        Assert.True(response1.StatusCode == System.Net.HttpStatusCode.Conflict || response2.StatusCode == System.Net.HttpStatusCode.Conflict);

        using(var scope = CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
            var roomSlotCounter = dbContext.RoomSlots.Count();
            Assert.Equal(1, roomSlotCounter);
        }
    }
}