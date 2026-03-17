using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Tech._4Manager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerNavigationToTimesheet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_timesheets_CustomerId",
                table: "timesheets",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_timesheets_customers_CustomerId",
                table: "timesheets",
                column: "CustomerId",
                principalTable: "customers",
                principalColumn: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_timesheets_customers_CustomerId",
                table: "timesheets");

            migrationBuilder.DropIndex(
                name: "IX_timesheets_CustomerId",
                table: "timesheets");
        }
    }
}
