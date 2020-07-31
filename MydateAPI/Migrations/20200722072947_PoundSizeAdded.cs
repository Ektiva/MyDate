using Microsoft.EntityFrameworkCore.Migrations;

namespace MydateAPI.Migrations
{
    public partial class PoundSizeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "height",
            //    table: "Users");

            migrationBuilder.RenameColumn(
                name: "pound",
                table: "Users",
                newName: "Pound");

            migrationBuilder.AddColumn<int>(
                name: "Feet",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Size",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Feet",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Pound",
                table: "Users",
                newName: "pound");

            //migrationBuilder.AddColumn<string>(
            //    name: "height",
            //    table: "Users",
            //    type: "TEXT",
            //    nullable: true);
        }
    }
}
