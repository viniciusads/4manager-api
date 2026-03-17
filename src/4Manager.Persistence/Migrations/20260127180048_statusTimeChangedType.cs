using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Tech._4Manager.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class statusTimeChangedType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "projects"
                ALTER COLUMN "StatusTime"
                TYPE interval
                USING "StatusTime" - timestamptz '1970-01-01 00:00:00+00';
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE "projects"
                ALTER COLUMN "StatusTime"
                TYPE timestamp with time zone
                USING timestamptz '1970-01-01 00:00:00+00' + "StatusTime";
            """);
        }
    }
}
