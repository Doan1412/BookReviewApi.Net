using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookReview.Migrations
{
    /// <inheritdoc />
    public partial class refreshtokentable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "refreshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_refreshTokens_UserId",
                table: "refreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_refreshTokens_users_UserId",
                table: "refreshTokens",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refreshTokens_users_UserId",
                table: "refreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_refreshTokens_UserId",
                table: "refreshTokens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "refreshTokens");
        }
    }
}
