using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoviesAdmin.Migrations
{
    /// <inheritdoc />
    public partial class Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CriticReview_MovieId",
                table: "CriticReview");

            migrationBuilder.CreateIndex(
                name: "IX_CriticReview_MovieId_CreatedBy",
                table: "CriticReview",
                columns: new[] { "MovieId", "CreatedBy" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CriticReview_MovieId_CreatedBy",
                table: "CriticReview");

            migrationBuilder.CreateIndex(
                name: "IX_CriticReview_MovieId",
                table: "CriticReview",
                column: "MovieId");
        }
    }
}
