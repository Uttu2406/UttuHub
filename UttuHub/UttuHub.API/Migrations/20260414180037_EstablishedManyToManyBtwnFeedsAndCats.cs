using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UttuHub.API.Migrations
{
    /// <inheritdoc />
    public partial class EstablishedManyToManyBtwnFeedsAndCats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedItems_Categories_CategoryId",
                table: "FeedItems");

            migrationBuilder.DropIndex(
                name: "IX_FeedItems_CategoryId",
                table: "FeedItems");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "FeedItems");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Contacts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FeedItemCategories",
                columns: table => new
                {
                    FeedItemId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedItemCategories", x => new { x.FeedItemId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_FeedItemCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedItemCategories_FeedItems_FeedItemId",
                        column: x => x.FeedItemId,
                        principalTable: "FeedItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_UserId",
                table: "Contacts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedItemCategories_CategoryId",
                table: "FeedItemCategories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Users_UserId",
                table: "Contacts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Users_UserId",
                table: "Contacts");

            migrationBuilder.DropTable(
                name: "FeedItemCategories");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_UserId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Contacts");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "FeedItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FeedItems_CategoryId",
                table: "FeedItems",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedItems_Categories_CategoryId",
                table: "FeedItems",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
