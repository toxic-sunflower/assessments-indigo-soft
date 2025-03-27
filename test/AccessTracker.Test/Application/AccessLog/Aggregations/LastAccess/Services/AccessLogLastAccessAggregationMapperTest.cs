using AccessTracker.Application.AccessLog.Aggregations.LastAccess.Services;
using AccessTracker.Domain.AccessLog;
using FluentAssertions;

namespace AccessTracker.Test.Application.AccessLog.Aggregations.LastAccess.Services;

public class AccessLogLastAccessAggregationMapperTest : TestBase<AccessLogLastAccessAggregationMapper>
{
    [Fact(DisplayName = "Должен маппить корректно")]
    public void Should_Map_Correctly()
    {
        // Arrange
        var accessLogEntry = new AccessLogEntry
        {
            UserId = 1017,
            IpAddress = "127.0.0.1"
        };

        
        // Act
        var result = Target.MapLastAccessAggregation(accessLogEntry);
        
        
        // Assert
        result.UserId.Should().Be(accessLogEntry.UserId);
        result.LastIpAddress.Should().Be(accessLogEntry.IpAddress);
        result.LastAccessUtcTime.Should().Be(accessLogEntry.Timestamp);
    }
}