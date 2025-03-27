﻿// <auto-generated />
using System;
using AccessTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AccessTracker.Data.Migrations
{
    [DbContext(typeof(AccessTrackerDbContext))]
    [Migration("20250327032255_AddAggregations")]
    partial class AddAggregations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AccessTracker.Domain.AccessLog.AccessLogEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Timestamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("timezone('utc', now())");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("AccessLog", (string)null);
                });

            modelBuilder.Entity("AccessTracker.Domain.AccessLog.Aggregations.Common.Checkpoints.AccessLogAggregationCheckpoint", b =>
                {
                    b.Property<string>("AggregationType")
                        .HasColumnType("text");

                    b.Property<long>("LastAggregatedEventId")
                        .HasColumnType("bigint");

                    b.HasKey("AggregationType");

                    b.ToTable("AccessLogAggregationCheckpoints", (string)null);
                });

            modelBuilder.Entity("AccessTracker.Domain.AccessLog.Aggregations.LastAccess.AccessLogLastAccessAggregation", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("UserId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("UserId"));

                    b.Property<DateTimeOffset>("LastAccessUtcTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("LastAccessUtcTime");

                    b.Property<string>("LastIpAddress")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("IpAddress");

                    b.HasKey("UserId");

                    b.ToTable("AccessLogLastAccessAggregations", (string)null);
                });

            modelBuilder.Entity("AccessTracker.Domain.AccessLog.Aggregations.UserIpLink.AccessLogUserIpLinkAggregation", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("UserId");

                    b.Property<string>("IpAddress")
                        .HasColumnType("text")
                        .HasColumnName("IpAddress");

                    b.HasKey("UserId", "IpAddress");

                    b.ToTable("AccessLogUserIpLinkAggregations", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
