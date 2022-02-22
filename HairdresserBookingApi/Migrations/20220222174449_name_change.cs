using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HairdresserBookingApi.Migrations
{
    public partial class name_change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_WorkerServices_WorkerServiceId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "WorkerServices");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_WorkerServiceId",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "WorkerActivityId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsForMan = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerActivities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<double>(type: "float", nullable: false),
                    RequiredMinutes = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerActivities_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerActivities_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_WorkerActivityId",
                table: "Reservations",
                column: "WorkerActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerActivities_ActivityId",
                table: "WorkerActivities",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerActivities_WorkerId",
                table: "WorkerActivities",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_WorkerActivities_WorkerActivityId",
                table: "Reservations",
                column: "WorkerActivityId",
                principalTable: "WorkerActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_WorkerActivities_WorkerActivityId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "WorkerActivities");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_WorkerActivityId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "WorkerActivityId",
                table: "Reservations");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsForMan = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkerServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    RequiredMinutes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkerServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerServices_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_WorkerServiceId",
                table: "Reservations",
                column: "WorkerServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerServices_ServiceId",
                table: "WorkerServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerServices_WorkerId",
                table: "WorkerServices",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_WorkerServices_WorkerServiceId",
                table: "Reservations",
                column: "WorkerServiceId",
                principalTable: "WorkerServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
