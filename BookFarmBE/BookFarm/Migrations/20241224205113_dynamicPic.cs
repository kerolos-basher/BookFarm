using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookFarm.Migrations
{
    /// <inheritdoc />
    public partial class dynamicPic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "placeID",
                table: "books",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "carouselItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaptionTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaptionText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carouselItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceForNight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_places", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_books_places_placeID",
                table: "books");

            migrationBuilder.DropTable(
                name: "carouselItems");

            migrationBuilder.DropTable(
                name: "places");

            migrationBuilder.DropIndex(
                name: "IX_books_placeID",
                table: "books");

            migrationBuilder.DropColumn(
                name: "placeID",
                table: "books");
        }
    }
}
