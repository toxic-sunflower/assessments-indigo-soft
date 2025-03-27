using Microsoft.EntityFrameworkCore.Storage;

namespace AccessTracker.Data.Storages;

public class Storage : IStorage
{
    public Task<IDbContextTransaction> BeginTransactionAsync(
        AccessTrackerDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return dbContext.Database.BeginTransactionAsync(cancellationToken);
    }
}