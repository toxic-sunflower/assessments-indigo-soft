using AccessTracker.Application.AccessLog.Aggregations.UserIpLinks.Services;
using FluentAssertions;

namespace AccessTracker.Test.Application.AccessLog.Aggregations.UserIpLink.Services;

public class AccessLogUserIpLinkAggregationMapperTest :
    TestBase<AccessLogUserIpLinkAggregationMapper>
{
    [Fact(DisplayName = "Должен маппить корректно")]
    public void Should_Map_Correctly()
    {
        // Arrange
        const long userId = 1017;
        string[] ipAddresses = ["127.0.0.1", "192.168.0.1"];
        
        
        // Act
        var result = Target.MapLastAccessAggregation(userId, ipAddresses);
        
        
        // Assert
        result.Count.Should().Be(ipAddresses.Length);
        result.All(x => ipAddresses.Contains(x.IpAddress)).Should().BeTrue();
    }
}