using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AccessTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAggregations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccessLogAggregationCheckpoints",
                columns: table => new
                {
                    AggregationType = table.Column<string>(type: "text", nullable: false),
                    LastAggregatedEventId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLogAggregationCheckpoints", x => x.AggregationType);
                });

            migrationBuilder.CreateTable(
                name: "AccessLogLastAccessAggregations",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LastAccessUtcTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLogLastAccessAggregations", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AccessLogUserIpLinkAggregations",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IpAddress = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessLogUserIpLinkAggregations", x => new { x.UserId, x.IpAddress });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessLogAggregationCheckpoints");

            migrationBuilder.DropTable(
                name: "AccessLogLastAccessAggregations");

            migrationBuilder.DropTable(
                name: "AccessLogUserIpLinkAggregations");
        }
    }
}
