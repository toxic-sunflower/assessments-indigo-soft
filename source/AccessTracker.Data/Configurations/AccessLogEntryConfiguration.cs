using AccessTracker.Domain.AccessLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessTracker.Data.Configurations;


public class AccessLogEntryConfiguration : IEntityTypeConfiguration<AccessLogEntry>
{
    public void Configure(EntityTypeBuilder<AccessLogEntry> builder)
    {
        builder.ToTable("AccessLog");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.IpAddress)
            .IsRequired();

        builder.Property(x => x.Timestamp)
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("timezone('utc', now())");
    }
}