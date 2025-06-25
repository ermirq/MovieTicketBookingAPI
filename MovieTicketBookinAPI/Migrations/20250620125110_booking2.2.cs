using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieTicketBookinAPI.Migrations
{
    /// <inheritdoc />
    public partial class booking22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeatNumber",
                table: "Seats",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "Seats");
        }
    }
}
