using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookFarm.Migrations
{
    /// <inheritdoc />
    public partial class placeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_books_places_placeID",
                table: "books");

            migrationBuilder.DropIndex(
                name: "IX_books_placeID",
                table: "books");

            migrationBuilder.AlterColumn<int>(
                name: "placeID",
                table: "books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PlacesId",
                table: "books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_books_PlacesId",
                table: "books",
                column: "PlacesId");

            migrationBuilder.AddForeignKey(
                name: "FK_books_places_PlacesId",
                table: "books",
                column: "PlacesId",
                principalTable: "places",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_books_places_PlacesId",
                table: "books");

            migrationBuilder.DropIndex(
                name: "IX_books_PlacesId",
                table: "books");

            migrationBuilder.DropColumn(
                name: "PlacesId",
                table: "books");

            migrationBuilder.AlterColumn<int>(
                name: "placeID",
                table: "books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_books_placeID",
                table: "books",
                column: "placeID");

            migrationBuilder.AddForeignKey(
                name: "FK_books_places_placeID",
                table: "books",
                column: "placeID",
                principalTable: "places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
