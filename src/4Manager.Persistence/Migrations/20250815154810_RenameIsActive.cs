using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Manager.Persistence.Migrations
{
    public partial class RenameIsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM information_schema.tables 
                              WHERE table_schema = 'public' AND table_name = 'Usuarios')
                       AND NOT EXISTS (SELECT 1 FROM information_schema.tables 
                                      WHERE table_schema = 'public' AND table_name = 'users') THEN
                        ALTER TABLE ""Usuarios"" RENAME TO ""users"";
                    END IF;
                END $$;
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE IF EXISTS ""users""
                ADD COLUMN IF NOT EXISTS ""IsActive"" boolean NOT NULL DEFAULT true;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (SELECT 1 FROM information_schema.tables 
                              WHERE table_schema = 'public' AND table_name = 'users')
                       AND NOT EXISTS (SELECT 1 FROM information_schema.tables 
                                      WHERE table_schema = 'public' AND table_name = 'Usuarios') THEN
                        ALTER TABLE ""users"" RENAME TO ""Usuarios"";
                    END IF;
                END $$;
            ");

            migrationBuilder.Sql(@"ALTER TABLE ""Usuarios"" DROP COLUMN IF EXISTS ""IsActive"";");
        }
    }
}