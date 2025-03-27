using AccessTracker.Domain.AccessLog.Aggregations;

namespace AccessTracker.Application.AccessLog.Aggregations.Common.Services;

public interface IAccessLogAggregationTypeMapper
{
    AccessLogAggregationType GetAggregationType<TAggregation>();
    
    AccessLogAggregationType GetAggregationType(Type aggregationType);

    Type GetEntityType(AccessLogAggregationType aggregationType);
}