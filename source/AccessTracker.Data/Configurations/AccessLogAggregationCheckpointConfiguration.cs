using AccessTracker.Domain.AccessLog.Aggregations.Common.Checkpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessTracker.Data.Configurations;

public class AccessLogAggregationCheckpointConfiguration : IEntityTypeConfiguration<AccessLogAggregationCheckpoint>
{
    public void Configure(EntityTypeBuilder<AccessLogAggregationCheckpoint> builder)
    {
        builder.ToTable("AccessLogAggregationCheckpoints");

        builder.HasKey(x => x.AggregationType);

        builder.Property(x => x.AggregationType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(x => x.LastAggregatedEventId)
            .IsRequired();
    }
}
