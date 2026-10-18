using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Testcontainers.PostgreSql;

namespace BookingApi.Tests.IntegrationTests;

/// <summary>
/// A fixture that sets up a PostgreSQL database container for integration tests and provides a WebApplicationFactory for testing the Booking API.
/// Classes that implement IClassFixture<DatabaseFixture> will share the same instance of this fixture, allowing them to use the same database container and WebApplicationFactory.
/// Therefore the database container will be started only once for all tests that use this fixture, and it will be disposed of when all tests are finished.
/// </summary>
public class DatabaseFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithDatabase("testdb")
        .WithUsername("testuser")
        .WithPassword("testpassword")
        .Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", _dbContainer.GetConnectionString());
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
        await _dbContainer.DisposeAsync();
    }
}