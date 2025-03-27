using AccessTracker.Domain.AccessLog.Aggregations.UserIpLink;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessTracker.Data.Configurations;

public class AccessLogUserIpLinkAggregationConfiguration :
    IEntityTypeConfiguration<AccessLogUserIpLinkAggregation>
{
    public void Configure(EntityTypeBuilder<AccessLogUserIpLinkAggregation> builder)
    {
        builder.ToTable("AccessLogUserIpLinkAggregations");
        builder.HasKey(x => new {x.UserId, x.IpAddress});
        builder.Property(x => x.UserId).HasColumnName("UserId");
        builder.Property(x => x.IpAddress).HasColumnName("IpAddress");
        builder.HasIndex(x => new {x.UserId, x.IpAddress}).IsUnique();
    }
}