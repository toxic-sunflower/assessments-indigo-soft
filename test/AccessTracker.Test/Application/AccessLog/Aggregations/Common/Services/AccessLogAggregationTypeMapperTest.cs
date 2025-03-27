using AccessTracker.Application.AccessLog.Aggregations.Common.Services;
using AccessTracker.Domain.AccessLog.Aggregations;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;
using AccessTracker.Domain.AccessLog.Aggregations.UserIpLink;
using FluentAssertions;

namespace AccessTracker.Test.Application.AccessLog.Aggregations.Common.Services;

public class AccessLogAggregationTypeMapperTest : TestBase<AccessLogAggregationTypeMapper>
{
    [Theory(DisplayName = "GetAggregationType должен крректно выпполнять маппинг")]
    [InlineData(typeof(AccessLogLastAccessAggregation), AccessLogAggregationType.LastAccess)]
    [InlineData(typeof(AccessLogUserIpLinkAggregation), AccessLogAggregationType.UserIpLink)]
    public void GetAggregationType_Should_Map_Correct_Result(
        Type entityType,
        AccessLogAggregationType expectedAggregationType) 
    {
        Target.GetAggregationType(entityType)
            .Should()
            .Be(expectedAggregationType);
    }
    
    
    [Theory(DisplayName = "GetEntityType должен крректно выпполнять маппинг")]
    [InlineData(AccessLogAggregationType.LastAccess, typeof(AccessLogLastAccessAggregation))]
    [InlineData(AccessLogAggregationType.UserIpLink, typeof(AccessLogUserIpLinkAggregation))]
    public void GetEntityType_Should_Map_Correct_Result(
        AccessLogAggregationType aggregationType,
        Type expectedEntityType) 
    {
        Target.GetEntityType(aggregationType)
            .Should()
            .Be(expectedEntityType);
    }
}