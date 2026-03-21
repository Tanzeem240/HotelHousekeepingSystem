using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelHousekeepingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AssignedToUserId",
                table: "CleaningTasks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTasks_AssignedToUserId",
                table: "CleaningTasks",
                column: "AssignedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CleaningTasks_RoomId",
                table: "CleaningTasks",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTasks_Rooms_RoomId",
                table: "CleaningTasks",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CleaningTasks_Users_AssignedToUserId",
                table: "CleaningTasks",
                column: "AssignedToUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CleaningTasks_Rooms_RoomId",
                table: "CleaningTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_CleaningTasks_Users_AssignedToUserId",
                table: "CleaningTasks");

            migrationBuilder.DropIndex(
                name: "IX_CleaningTasks_AssignedToUserId",
                table: "CleaningTasks");

            migrationBuilder.DropIndex(
                name: "IX_CleaningTasks_RoomId",
                table: "CleaningTasks");

            migrationBuilder.AlterColumn<int>(
                name: "AssignedToUserId",
                table: "CleaningTasks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
