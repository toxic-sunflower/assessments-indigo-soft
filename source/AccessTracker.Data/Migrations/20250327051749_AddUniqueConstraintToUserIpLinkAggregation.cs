using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToUserIpLinkAggregation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AccessLogUserIpLinkAggregations_UserId_IpAddress",
                table: "AccessLogUserIpLinkAggregations",
                columns: new[] { "UserId", "IpAddress" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccessLogUserIpLinkAggregations_UserId_IpAddress",
                table: "AccessLogUserIpLinkAggregations");
        }
    }
}
