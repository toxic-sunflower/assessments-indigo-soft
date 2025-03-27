namespace AccessTracker.Application.EventTracking;

public class AccessEventTrackingOptions
{
    public required int AttemptsCount { get; set; }
    
    public required TimeSpan RetryDelay { get; set; }
}