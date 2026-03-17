using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Tech._4Manager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class newActivityTypeRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_timesheets_activities_ActivityTypeId",
                table: "timesheets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_activities",
                table: "activities");

            migrationBuilder.RenameTable(
                name: "activities",
                newName: "activityTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_activityTypes",
                table: "activityTypes",
                column: "ActivityTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_timesheets_activityTypes_ActivityTypeId",
                table: "timesheets",
                column: "ActivityTypeId",
                principalTable: "activityTypes",
                principalColumn: "ActivityTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_timesheets_activityTypes_ActivityTypeId",
                table: "timesheets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_activityTypes",
                table: "activityTypes");

            migrationBuilder.RenameTable(
                name: "activityTypes",
                newName: "activities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_activities",
                table: "activities",
                column: "ActivityTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_timesheets_activities_ActivityTypeId",
                table: "timesheets",
                column: "ActivityTypeId",
                principalTable: "activities",
                principalColumn: "ActivityTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
