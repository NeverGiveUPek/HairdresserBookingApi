using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairdresserBookingApi.Migrations
{
    public partial class _22022022_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkBreak_WorkerAvailabilities_AvailabilityId",
                table: "WorkBreak");

            migrationBuilder.DropIndex(
                name: "IX_WorkBreak_AvailabilityId",
                table: "WorkBreak");

            migrationBuilder.DropColumn(
                name: "AvailabilityId",
                table: "WorkBreak");

            migrationBuilder.RenameColumn(
                name: "WorkerAvailability",
                table: "WorkBreak",
                newName: "WorkerAvailabilityId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkBreak_WorkerAvailabilityId",
                table: "WorkBreak",
                column: "WorkerAvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkBreak_WorkerAvailabilities_WorkerAvailabilityId",
                table: "WorkBreak",
                column: "WorkerAvailabilityId",
                principalTable: "WorkerAvailabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkBreak_WorkerAvailabilities_WorkerAvailabilityId",
                table: "WorkBreak");

            migrationBuilder.DropIndex(
                name: "IX_WorkBreak_WorkerAvailabilityId",
                table: "WorkBreak");

            migrationBuilder.RenameColumn(
                name: "WorkerAvailabilityId",
                table: "WorkBreak",
                newName: "WorkerAvailability");

            migrationBuilder.AddColumn<int>(
                name: "AvailabilityId",
                table: "WorkBreak",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WorkBreak_AvailabilityId",
                table: "WorkBreak",
                column: "AvailabilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkBreak_WorkerAvailabilities_AvailabilityId",
                table: "WorkBreak",
                column: "AvailabilityId",
                principalTable: "WorkerAvailabilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
