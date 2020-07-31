using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MydateAPI.Migrations
{
    public partial class UserUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Jobtitle",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lastname",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mood",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Jobtitle",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Lastname",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Mood",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");
        }
    }
}
