using BookingApi.Application.Abstractions;

namespace BookingApi.Persistence;

public class UnitOfWork(BookingDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
