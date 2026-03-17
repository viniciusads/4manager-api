using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Tech._4Manager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class activityTableCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "activities",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ActivityName = table.Column<string>(type: "text", nullable: false),
                    ActivityColor = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activities", x => x.ActivityId);
                });

            migrationBuilder.AlterColumn<Guid>(
                name: "ActivityId",
                table: "timesheets",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_timesheets_ActivityId",
                table: "timesheets",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_timesheets_activities_ActivityId",
                table: "timesheets",
                column: "ActivityId",
                principalTable: "activities",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_timesheets_activities_ActivityId",
                table: "timesheets");

            migrationBuilder.DropTable(
                name: "activities");

            migrationBuilder.DropIndex(
                name: "IX_timesheets_ActivityId",
                table: "timesheets");

            migrationBuilder.AlterColumn<Guid>(
                name: "ActivityId",
                table: "timesheets",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}