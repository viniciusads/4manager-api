using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Tech._4Manager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changeRoleColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "users",
                newName: "Function");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Function",
                table: "users",
                newName: "Role");
        }
    }
}
