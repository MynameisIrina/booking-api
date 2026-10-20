using System.Net.Http.Json;
using BookingApi.Domain.Entities;
using BookingApi.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApi.Tests.IntegrationTests;

public class RoomSlotTests(DatabaseFixture databaseFixture) : IClassFixture<DatabaseFixture>, IAsyncLifetime
{
    private readonly DatabaseFixture sharedContext = databaseFixture;

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public async Task InitializeAsync()
    {
        using var scope = CreateScope();
        await scope.ServiceProvider.GetRequiredService<BookingDbContext>().Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Bookings\", \"RoomSlots\";");
    }

    private IServiceScope CreateScope() => sharedContext.Services.CreateScope();

    [Fact]
    public async Task AddSameRoomSlotsToDb_ShouldReturnDbUpdateException()
    {
        DateTime now = DateTime.UtcNow;
        using (var scope = CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
            dbContext.Add(new RoomSlot("Name", now));
            await dbContext.SaveChangesAsync();
        }

        await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            using (var scope = CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
                dbContext.Add(new RoomSlot("Name", now));
                await dbContext.SaveChangesAsync();
            }
        });
    }
}