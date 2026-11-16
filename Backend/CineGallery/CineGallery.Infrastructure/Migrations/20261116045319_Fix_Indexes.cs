using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CineGallery.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Indexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movies_Id",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_Name",
                table: "Movies");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Name",
                table: "Movies",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Movies_Name",
                table: "Movies");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Id",
                table: "Movies",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Movies_Name",
                table: "Movies",
                column: "Name");
        }
    }
}
