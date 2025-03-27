using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintToCheckpoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AccessLogAggregationCheckpoints_AggregationType",
                table: "AccessLogAggregationCheckpoints",
                column: "AggregationType",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AccessLogAggregationCheckpoints_AggregationType",
                table: "AccessLogAggregationCheckpoints");
        }
    }
}
