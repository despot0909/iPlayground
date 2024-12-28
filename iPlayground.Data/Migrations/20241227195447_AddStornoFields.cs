using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace iPlayground.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStornoFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStorno",
                table: "Sessions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "StornoReason",
                table: "Sessions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StornoTime",
                table: "Sessions",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStorno",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "StornoReason",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "StornoTime",
                table: "Sessions");
        }
    }
}
