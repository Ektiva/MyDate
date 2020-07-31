using Microsoft.EntityFrameworkCore.Migrations;

namespace MydateAPI.Migrations
{
    public partial class SmokeDrinkAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Drink",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Smoke",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Drink",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Smoke",
                table: "Users");
        }
    }
}
