using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelHousekeepingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignmentFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedUserId",
                table: "CleaningTasks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTasks_AssignedUserId",
                table: "CleaningTasks",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTasks_Users_AssignedUserId",
                table: "CleaningTasks",
                column: "AssignedUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CleaningTasks_Users_AssignedUserId",
                table: "CleaningTasks");

            migrationBuilder.DropIndex(
                name: "IX_CleaningTasks_AssignedUserId",
                table: "CleaningTasks");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "CleaningTasks");
        }
    }
}
