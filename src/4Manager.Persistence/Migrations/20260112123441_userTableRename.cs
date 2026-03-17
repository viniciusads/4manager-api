using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _4Tech._4Manager.Persistence.Migrations
{
    public partial class userTableRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_users_CustomerManagerId",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_team_collaborators_users_CollaboratorId",
                table: "team_collaborators");

            migrationBuilder.DropForeignKey(
                name: "FK_teams_users_ManagerId",
                table: "teams");

            migrationBuilder.DropForeignKey(
                name: "FK_timesheets_users_UserId",
                table: "timesheets");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "userProfiles");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_userProfiles_CustomerManagerId",
                table: "projects",
                column: "CustomerManagerId",
                principalTable: "userProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_team_collaborators_userProfiles_CollaboratorId",
                table: "team_collaborators",
                column: "CollaboratorId",
                principalTable: "userProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_teams_userProfiles_ManagerId",
                table: "teams",
                column: "ManagerId",
                principalTable: "userProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_timesheets_userProfiles_UserId",
                table: "timesheets",
                column: "UserId",
                principalTable: "userProfiles",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_userProfiles_CustomerManagerId",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_team_collaborators_userProfiles_CollaboratorId",
                table: "team_collaborators");

            migrationBuilder.DropForeignKey(
                name: "FK_teams_userProfiles_ManagerId",
                table: "teams");

            migrationBuilder.DropForeignKey(
                name: "FK_timesheets_userProfiles_UserId",
                table: "timesheets");

            migrationBuilder.RenameTable(
                name: "userProfiles",
                newName: "users");

            migrationBuilder.RenameIndex(
                name: "PK_userProfiles",
                table: "users",
                newName: "PK_users");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_users_CustomerManagerId",
                table: "projects",
                column: "CustomerManagerId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_team_collaborators_users_CollaboratorId",
                table: "team_collaborators",
                column: "CollaboratorId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_teams_users_ManagerId",
                table: "teams",
                column: "ManagerId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_timesheets_users_UserId",
                table: "timesheets",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
