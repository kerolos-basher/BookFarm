using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookFarm.Migrations
{
    /// <inheritdoc />
    public partial class withDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateFrom",
                table: "books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DateTo",
                table: "books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFrom",
                table: "books");

            migrationBuilder.DropColumn(
                name: "DateTo",
                table: "books");
        }
    }
}
