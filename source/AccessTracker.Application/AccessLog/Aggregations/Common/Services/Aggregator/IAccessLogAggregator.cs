namespace AccessTracker.Application.AccessLog.Aggregations.Common.Services.Aggregator;

public interface IAccessLogAggregator
{
    TimeSpan GetInterval();
    
    Task AggregateAsync(CancellationToken cancellationToken);
}

public interface IAccessLogAggregator<TAggregation> : IAccessLogAggregator;