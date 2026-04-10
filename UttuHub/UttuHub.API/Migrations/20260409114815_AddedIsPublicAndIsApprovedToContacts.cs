using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UttuHub.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsPublicAndIsApprovedToContacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Contacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Contacts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Contacts");
        }
    }
}
