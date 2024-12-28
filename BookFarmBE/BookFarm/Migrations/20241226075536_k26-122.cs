using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookFarm.Migrations
{
    /// <inheritdoc />
    public partial class k26122 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "carouselItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "carouselItems",
                keyColumn: "Id",
                keyValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "carouselItems",
                columns: new[] { "Id", "CaptionText", "CaptionTitle", "ImageUrl" },
                values: new object[,]
                {
                    { 1, "Some representative content for the first slide.", "First Slide", "img/1    .jpeg" },
                    { 2, "Some representative content for the second slide.", "Second Slide", "img/2.jpeg" }
                });
        }
    }
}
