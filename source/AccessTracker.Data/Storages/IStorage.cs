using Microsoft.EntityFrameworkCore.Storage;

namespace AccessTracker.Data.Storages;

public interface IStorage
{
    Task<IDbContextTransaction> BeginTransactionAsync(
        AccessTrackerDbContext dbContext,
        CancellationToken cancellationToken);
}