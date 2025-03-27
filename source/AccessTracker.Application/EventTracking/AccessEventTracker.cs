using AccessTracker.Application.EventTracking.Services;
using AccessTracker.Domain.AccessLog;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace AccessTracker.Application.EventTracking;


public class AccessEventTracker : IAccessEventTracker
{
    private readonly IOptions<AccessEventTrackingOptions> _options;
    private readonly IAccessLogRepository _repository;
    private readonly IAccessEventTrackFailedHandler _eventTrackFailedHandler;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<AccessEventTracker> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;



    public AccessEventTracker(
        IOptions<AccessEventTrackingOptions> options,
        IAccessLogRepository repository,
        IAccessEventTrackFailedHandler eventTrackFailedHandler,
        TimeProvider timeProvider,
        ILogger<AccessEventTracker> logger)
    {
        _options = options;
        _repository = repository;
        _eventTrackFailedHandler = eventTrackFailedHandler;
        _timeProvider = timeProvider;
        _logger = logger;

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: options.Value.AttemptsCount,
                sleepDurationProvider: attempt => options.Value.RetryDelay * attempt,
                onRetry: OnRetry);
    }

    public async Task<ErrorOr<Success>> TrackAccessEventAsync(AccessEvent evt,
        CancellationToken cancellationToken)
    {
        var registrationTime = _timeProvider.GetUtcNow();

        var entry = new AccessLogEntry
        {
            UserId = evt.UserId,
            IpAddress = evt.IpAddress,
            Timestamp = registrationTime
        };

        try
        {
            await _retryPolicy.ExecuteAsync(
                async cancellation =>
                {
                    await _repository.AddEntryAsync(entry, cancellation);
                },
                cancellationToken);

            return new Success();
        }
        catch (Exception exception)
        {
            await _eventTrackFailedHandler.HandleTrackFailureAsync(
                exception,
                evt.UserId,
                evt.IpAddress,
                registrationTime,
                cancellationToken);

            return Error.Unexpected(
                code: "Error.AccessTracker.TrackEvent.Unexpected",
                description: exception.ToString());
        }
    }
    
    private void OnRetry(
        Exception exception,
        TimeSpan timeSpan,
        int retryCount,
        Context _)
    {
        _logger.LogWarning(
            exception,
            "Retry {Attempt}: Failed to track access event. Retrying in {Delay}...",
            retryCount,
            timeSpan);
    }
}