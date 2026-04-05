using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelHousekeepingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddClockFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClockInTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClockOutTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClockInTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ClockOutTime",
                table: "Users");
        }
    }
}
