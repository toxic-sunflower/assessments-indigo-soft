using AccessTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace AccessTracker.Services;


public class MigrationService : IHostedService
{
    private readonly IDbContextFactory<AccessTrackerDbContext> _dbContextFactory;
    private readonly ILogger<MigrationService> _logger;

    public MigrationService(
        ILogger<MigrationService> logger,
        IDbContextFactory<AccessTrackerDbContext> dbContextFactory)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
    }

    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        _logger.LogInformation("📦 Применение миграций для {Context}...", nameof(AccessTrackerDbContext));
        
        await context.Database.MigrateAsync(cancellationToken);
        
        _logger.LogInformation("✅ Миграции применены.");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}