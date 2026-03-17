using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Tech._4Manager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "schedules",
                columns: table => new
                {
                    ScheduleId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schedules", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK_schedules_projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "projects",
                        principalColumn: "ProjectId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scheduleTasks",
                columns: table => new
                {
                    ScheduleTaskId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ScheduleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParentTaskId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaskType = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ActualStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ResponsibleId = table.Column<Guid>(type: "uuid", nullable: true),
                    Stakeholder = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CompletionPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    OrderIndex = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scheduleTasks", x => x.ScheduleTaskId);
                    table.ForeignKey(
                        name: "FK_scheduleTasks_scheduleTasks_ParentTaskId",
                        column: x => x.ParentTaskId,
                        principalTable: "scheduleTasks",
                        principalColumn: "ScheduleTaskId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_scheduleTasks_schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "schedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_scheduleTasks_userProfiles_ResponsibleId",
                        column: x => x.ResponsibleId,
                        principalTable: "userProfiles",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_schedules_ProjectId",
                table: "schedules",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_scheduleTasks_ParentTaskId",
                table: "scheduleTasks",
                column: "ParentTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_scheduleTasks_ResponsibleId",
                table: "scheduleTasks",
                column: "ResponsibleId");

            migrationBuilder.CreateIndex(
                name: "IX_scheduleTasks_ScheduleId",
                table: "scheduleTasks",
                column: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "scheduleTasks");

            migrationBuilder.DropTable(
                name: "schedules");
        }
    }
}
