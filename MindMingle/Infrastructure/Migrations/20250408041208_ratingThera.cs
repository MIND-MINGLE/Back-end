using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ratingThera : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TherapistId",
                table: "Ratings",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_TherapistId",
                table: "Ratings",
                column: "TherapistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Therapists_TherapistId",
                table: "Ratings",
                column: "TherapistId",
                principalTable: "Therapists",
                principalColumn: "TherapistId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Therapists_TherapistId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_TherapistId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "TherapistId",
                table: "Ratings");
        }
    }
}
