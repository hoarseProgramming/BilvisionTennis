using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BilvisionTennisAPI.GraphQL.Migrations
{
    /// <inheritdoc />
    public partial class Betterments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UmpireId",
                table: "Matches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_UmpireId",
                table: "Matches",
                column: "UmpireId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Umpires_UmpireId",
                table: "Matches",
                column: "UmpireId",
                principalTable: "Umpires",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Umpires_UmpireId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_UmpireId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "UmpireId",
                table: "Matches");
        }
    }
}
