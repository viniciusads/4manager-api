using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Tech._4Manager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class timesheetProjectIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_timesheets_ProjectId",
                table: "timesheets",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_timesheets_projects_ProjectId",
                table: "timesheets",
                column: "ProjectId",
                principalTable: "projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_timesheets_projects_ProjectId",
                table: "timesheets");

            migrationBuilder.DropIndex(
                name: "IX_timesheets_ProjectId",
                table: "timesheets");
        }
    }
}
