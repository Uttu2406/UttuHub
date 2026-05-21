using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UttuHub.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedThreeMissingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "FeedItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "FeedItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "FeedItems");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "FeedItems");
        }
    }
}
