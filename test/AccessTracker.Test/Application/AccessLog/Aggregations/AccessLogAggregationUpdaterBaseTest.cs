using AccessTracker.Application.AccessLog.Aggregations;
using AccessTracker.Application.AccessLog.Aggregations.Common.Services;
using AccessTracker.Application.AccessLog.Aggregations.Common.Services.Updater;
using AccessTracker.Data;
using AccessTracker.Data.Storages;
using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace AccessTracker.Test.Application.AccessLog.Aggregations;


public class AccessLogAggregationUpdaterBaseTest : TestBase<TestUpdater>
{
    private const AccessLogAggregationType AggregationType = AccessLogAggregationType.LastAccess;
    
    
    [Fact(DisplayName = "Должен обновлять агрегации и ID последней агрегированной записи в транзакции")]
    public async Task Should_Update_Aggregations_And_LastAggregatedEventId_In_Transaction()
    {
        // Arrange
        AccessLogEntry[] entries = [
            new ()
            {
                Id = 20,
                UserId = 0,
                IpAddress = "127.0.0.1"
            }
        ];
        
        var lastAggregatedEventId = entries[0].Id;
        
        var dbContext = new AccessTrackerDbContext(
            new DbContextOptionsBuilder<AccessTrackerDbContext>()
                .UseInMemoryDatabase("DB")
                .Options);

        _dbContextFactory
            .Setup(x => x.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(dbContext);
        
        
        // Act
        await Target.UpdateAggregationAsync(entries, CancellationToken.None);
        
        
        // Assert
        _storage.Verify(
            x => x.BeginTransactionAsync(dbContext, It.IsAny<CancellationToken>()),
            Times.Once);

        var (passedEntries, passedDbContext) = Target.GetPassedArguments();

        passedEntries.Should().BeSameAs(entries);
        passedDbContext.Should().BeSameAs(dbContext);
        
        _checkpointUpdater.Verify(
            x => x.UpdateAggregationCheckpointAsync(
                dbContext,
                AggregationType,
                lastAggregatedEventId,
                It.IsAny<CancellationToken>()),
            Times.Once);
        
        _transaction.Verify(
            x => x.CommitAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    
    protected override void ConfigureServices(IServiceCollection services)
    {
        _storage
            .Setup(x => x.BeginTransactionAsync(
                It.IsAny<AccessTrackerDbContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(_transaction.Object);

        _mapper
            .Setup(x => x.GetAggregationType<object>())
            .Returns(AggregationType);
        
        services.AddSingleton(_checkpointUpdater.Object);
        services.AddSingleton(_dbContextFactory.Object);
        services.AddSingleton(_storage.Object);
        services.AddSingleton(_mapper.Object);
    }


    private readonly Mock<IAccessLogAggregationCheckpointUpdater> _checkpointUpdater = new();
    private readonly Mock<IAccessLogAggregationTypeMapper> _mapper = new ();
    private readonly Mock<IDbContextFactory<AccessTrackerDbContext>> _dbContextFactory = new ();
    private readonly Mock<IStorage> _storage = new ();
    private readonly Mock<IDbContextTransaction> _transaction = new ();
}


public class TestUpdater : AccessLogAggregationUpdaterBase<object>
{
    private IReadOnlyList<AccessLogEntry> _accessLogEntries = null!;
    private AccessTrackerDbContext _dbContext = null!;


    public TestUpdater(
        IDbContextFactory<AccessTrackerDbContext> dbContextFactory,
        IAccessLogAggregationTypeMapper accessLogAggregationTypeMapper,
        IAccessLogAggregationCheckpointUpdater aggregationCheckpointUpdater,
        ILoggerFactory loggerFactory,
        IStorage storage) : 
            base(
                dbContextFactory,
                accessLogAggregationTypeMapper,
                aggregationCheckpointUpdater,
                loggerFactory,
                storage) { }

    
    protected override Task UpdateAggregationAsync(
        IReadOnlyList<AccessLogEntry> accessLogEntries,
        AccessTrackerDbContext dbContext,
        IStorage storage,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        (_accessLogEntries, _dbContext) = (accessLogEntries, dbContext);
        return Task.CompletedTask;
    }

    public (IReadOnlyList<AccessLogEntry>, AccessTrackerDbContext) GetPassedArguments() =>
        (_accessLogEntries, _dbContext);
}