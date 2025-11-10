using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Manager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Usuarios",
                newName: "isActive");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Usuarios"" 
                ALTER COLUMN ""Role"" TYPE integer 
                USING 0;
            ");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Usuarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Usuarios",
                newName: "Active");

            // Reversão: converter de integer para text
            migrationBuilder.Sql(@"
                ALTER TABLE ""Usuarios"" 
                ALTER COLUMN ""Role"" TYPE text 
                USING 'Analista';
            ");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Usuarios",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
