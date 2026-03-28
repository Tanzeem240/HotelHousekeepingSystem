using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelHousekeepingSystem.Migrations
{
    /// <inheritdoc />
    public partial class InitSupabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CleaningTasks_Users_AssignedToUserId",
                table: "CleaningTasks");

            migrationBuilder.DropIndex(
                name: "IX_CleaningTasks_AssignedToUserId",
                table: "CleaningTasks");

            migrationBuilder.DropColumn(
                name: "AssignedToUserId",
                table: "CleaningTasks");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CleaningTasks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsInspected",
                table: "CleaningTasks",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MaintenanceNotes",
                table: "CleaningTasks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CleaningTasks");

            migrationBuilder.DropColumn(
                name: "IsInspected",
                table: "CleaningTasks");

            migrationBuilder.DropColumn(
                name: "MaintenanceNotes",
                table: "CleaningTasks");

            migrationBuilder.AddColumn<int>(
                name: "AssignedToUserId",
                table: "CleaningTasks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTasks_AssignedToUserId",
                table: "CleaningTasks",
                column: "AssignedToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTasks_Users_AssignedToUserId",
                table: "CleaningTasks",
                column: "AssignedToUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
