using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AppoinmentPatchV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<string>(
                name: "GroupChatId",
                table: "Appointments",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_GroupChatId",
                table: "Appointments",
                column: "GroupChatId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_ChatGroups_GroupChatId",
                table: "Appointments",
                column: "GroupChatId",
                principalTable: "ChatGroups",
                principalColumn: "GroupChatId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_ChatGroups_GroupChatId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_GroupChatId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "GroupChatId",
                table: "Appointments");

         
        }
    }
}
