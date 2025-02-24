using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatGroups_UsersInGroups_UsersInGroupId1",
                table: "ChatGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Accounts_ClientId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_ChatGroups_ChatGroupId",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatGroups_UsersInGroupId1",
                table: "ChatGroups");

            migrationBuilder.DropColumn(
                name: "UsersInGroupId",
                table: "ChatGroups");

            migrationBuilder.DropColumn(
                name: "UsersInGroupId1",
                table: "ChatGroups");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ChatMessages",
                newName: "UsersInGroupId");

            migrationBuilder.RenameColumn(
                name: "ChatGroupId",
                table: "ChatMessages",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_ClientId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_UsersInGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_ChatGroupId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_AccountId");

            migrationBuilder.AddColumn<string>(
                name: "ChatGroupId1",
                table: "UsersInGroups",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Dob",
                table: "Patients",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInGroups_ChatGroupId1",
                table: "UsersInGroups",
                column: "ChatGroupId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Accounts_AccountId",
                table: "ChatMessages",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_UsersInGroups_UsersInGroupId",
                table: "ChatMessages",
                column: "UsersInGroupId",
                principalTable: "UsersInGroups",
                principalColumn: "UsersInGroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersInGroups_ChatGroups_ChatGroupId1",
                table: "UsersInGroups",
                column: "ChatGroupId1",
                principalTable: "ChatGroups",
                principalColumn: "ChatGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Accounts_AccountId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_UsersInGroups_UsersInGroupId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersInGroups_ChatGroups_ChatGroupId1",
                table: "UsersInGroups");

            migrationBuilder.DropIndex(
                name: "IX_UsersInGroups_ChatGroupId1",
                table: "UsersInGroups");

            migrationBuilder.DropColumn(
                name: "ChatGroupId1",
                table: "UsersInGroups");

            migrationBuilder.RenameColumn(
                name: "UsersInGroupId",
                table: "ChatMessages",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "ChatMessages",
                newName: "ChatGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_UsersInGroupId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_AccountId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_ChatGroupId");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Dob",
                table: "Patients",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<string>(
                name: "UsersInGroupId",
                table: "ChatGroups",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsersInGroupId1",
                table: "ChatGroups",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ChatGroups_UsersInGroupId1",
                table: "ChatGroups",
                column: "UsersInGroupId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatGroups_UsersInGroups_UsersInGroupId1",
                table: "ChatGroups",
                column: "UsersInGroupId1",
                principalTable: "UsersInGroups",
                principalColumn: "UsersInGroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Accounts_ClientId",
                table: "ChatMessages",
                column: "ClientId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_ChatGroups_ChatGroupId",
                table: "ChatMessages",
                column: "ChatGroupId",
                principalTable: "ChatGroups",
                principalColumn: "ChatGroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
