using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookinAPI.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureCascadeDeleteForCinemaSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Cinemas_CinemaId",
                table: "Seats");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Cinemas_CinemaId",
                table: "Seats",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seats_Cinemas_CinemaId",
                table: "Seats");

            migrationBuilder.AddForeignKey(
                name: "FK_Seats_Cinemas_CinemaId",
                table: "Seats",
                column: "CinemaId",
                principalTable: "Cinemas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
