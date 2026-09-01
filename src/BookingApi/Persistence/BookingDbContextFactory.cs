using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookingApi.Persistence
{
    /// <summary>
    /// This class is used by EF Core tools to create a DbContext instance at design time.
    /// </summary>
    public class BookingDbContextFactory : IDesignTimeDbContextFactory<BookingDbContext>
    {
        public BookingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookingDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=BookingDb;Username=postgres;Password=postgres");

            return new BookingDbContext(optionsBuilder.Options);
        }
    }
}