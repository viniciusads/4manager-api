using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Tech._4Manager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class activityTableRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_timesheets_activities_ActivityId",
                table: "timesheets");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "timesheets",
                newName: "ActivityTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_timesheets_ActivityId",
                table: "timesheets",
                newName: "IX_timesheets_ActivityTypeId");

            migrationBuilder.RenameColumn(
                name: "ActivityName",
                table: "activities",
                newName: "ActivityTypeName");

            migrationBuilder.RenameColumn(
                name: "ActivityColor",
                table: "activities",
                newName: "ActivityTypeColor");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "activities",
                newName: "ActivityTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_timesheets_activities_ActivityTypeId",
                table: "timesheets",
                column: "ActivityTypeId",
                principalTable: "activities",
                principalColumn: "ActivityTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_timesheets_activities_ActivityTypeId",
                table: "timesheets");

            migrationBuilder.RenameColumn(
                name: "ActivityTypeId",
                table: "timesheets",
                newName: "ActivityId");

            migrationBuilder.RenameIndex(
                name: "IX_timesheets_ActivityTypeId",
                table: "timesheets",
                newName: "IX_timesheets_ActivityId");

            migrationBuilder.RenameColumn(
                name: "ActivityTypeName",
                table: "activities",
                newName: "ActivityName");

            migrationBuilder.RenameColumn(
                name: "ActivityTypeColor",
                table: "activities",
                newName: "ActivityColor");

            migrationBuilder.RenameColumn(
                name: "ActivityTypeId",
                table: "activities",
                newName: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_timesheets_activities_ActivityId",
                table: "timesheets",
                column: "ActivityId",
                principalTable: "activities",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
