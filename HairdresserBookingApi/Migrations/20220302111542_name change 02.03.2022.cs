using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairdresserBookingApi.Migrations
{
    public partial class namechange02032022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkBreak_WorkerAvailabilities_WorkerAvailabilityId",
                table: "WorkBreak");

            migrationBuilder.RenameColumn(
                name: "WorkerAvailabilityId",
                table: "WorkBreak",
                newName: "AvailabilityId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkBreak_WorkerAvailabilityId",
                table: "WorkBreak",
                newName: "IX_WorkBreak_AvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkBreak_WorkerAvailabilities_AvailabilityId",
                table: "WorkBreak",
                column: "AvailabilityId",
                principalTable: "WorkerAvailabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkBreak_WorkerAvailabilities_AvailabilityId",
                table: "WorkBreak");

            migrationBuilder.RenameColumn(
                name: "AvailabilityId",
                table: "WorkBreak",
                newName: "WorkerAvailabilityId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkBreak_AvailabilityId",
                table: "WorkBreak",
                newName: "IX_WorkBreak_WorkerAvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkBreak_WorkerAvailabilities_WorkerAvailabilityId",
                table: "WorkBreak",
                column: "WorkerAvailabilityId",
                principalTable: "WorkerAvailabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
