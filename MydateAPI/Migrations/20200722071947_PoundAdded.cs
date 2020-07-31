using Microsoft.EntityFrameworkCore.Migrations;

namespace MydateAPI.Migrations
{
    public partial class PoundAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Pound",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Pound",
                table: "Users");
        }
    }
}
