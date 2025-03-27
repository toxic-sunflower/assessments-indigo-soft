using System.Reflection;
using AccessTracker.Domain;
using AccessTracker.Domain.AccessLog;
using AccessTracker.Domain.AccessLog.Aggregations;
using AccessTracker.Domain.AccessLog.Aggregations.Common.Checkpoints;
using AccessTracker.Domain.AccessLog.Aggregations.LastAccess;
using AccessTracker.Domain.AccessLog.Aggregations.UserIpLink;
using Microsoft.EntityFrameworkCore;

namespace AccessTracker.Data;

public class AccessTrackerDbContext : DbContext
{
    public AccessTrackerDbContext(DbContextOptions<AccessTrackerDbContext> options) : base(options) { }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetAssembly(
                typeof(AccessTrackerDbContext))!);
    
    
    public DbSet<AccessLogEntry> AccessLog { get; init; }
    
    public DbSet<AccessLogUserIpLinkAggregation> AccessLogUserIpLinkAggregations { get; init; }
    
    public DbSet<AccessLogLastAccessAggregation> AccessLogLastAccessAggregations { get; set; }
    
    public DbSet<AccessLogAggregationCheckpoint> AccessLogAggregationCheckpoints { get; set; }
}