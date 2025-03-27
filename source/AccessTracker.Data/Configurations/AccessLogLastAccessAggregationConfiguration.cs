using AccessTracker.Domain.AccessLog.Aggregations;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessTracker.Data.Configurations;

public class AccessLogLastAccessAggregationConfiguration :
    IEntityTypeConfiguration<AccessLogLastAccessAggregation>
{
    public void Configure(EntityTypeBuilder<AccessLogLastAccessAggregation> builder)
    {
        builder.ToTable("AccessLogLastAccessAggregations");
        builder.HasKey(x => x.UserId);
        builder.Property(x => x.UserId).HasColumnName("UserId").ValueGeneratedNever();
        builder.Property(x => x.LastIpAddress).HasColumnName("IpAddress");
        builder.Property(x => x.LastAccessUtcTime).HasColumnName("LastAccessUtcTime");
        builder.HasIndex(x => x.UserId).IsUnique();
    }
}