using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairdresserBookingApi.Migrations
{
    public partial class namechange02032022v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkBreak_WorkerAvailabilities_AvailabilityId",
                table: "WorkBreak");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerAvailabilities_Workers_WorkerId",
                table: "WorkerAvailabilities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkerAvailabilities",
                table: "WorkerAvailabilities");

            migrationBuilder.RenameTable(
                name: "WorkerAvailabilities",
                newName: "Availabilities");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerAvailabilities_WorkerId",
                table: "Availabilities",
                newName: "IX_Availabilities_WorkerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Availabilities",
                table: "Availabilities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Availabilities_Workers_WorkerId",
                table: "Availabilities",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkBreak_Availabilities_AvailabilityId",
                table: "WorkBreak",
                column: "AvailabilityId",
                principalTable: "Availabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Availabilities_Workers_WorkerId",
                table: "Availabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkBreak_Availabilities_AvailabilityId",
                table: "WorkBreak");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Availabilities",
                table: "Availabilities");

            migrationBuilder.RenameTable(
                name: "Availabilities",
                newName: "WorkerAvailabilities");

            migrationBuilder.RenameIndex(
                name: "IX_Availabilities_WorkerId",
                table: "WorkerAvailabilities",
                newName: "IX_WorkerAvailabilities_WorkerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkerAvailabilities",
                table: "WorkerAvailabilities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkBreak_WorkerAvailabilities_AvailabilityId",
                table: "WorkBreak",
                column: "AvailabilityId",
                principalTable: "WorkerAvailabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerAvailabilities_Workers_WorkerId",
                table: "WorkerAvailabilities",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
