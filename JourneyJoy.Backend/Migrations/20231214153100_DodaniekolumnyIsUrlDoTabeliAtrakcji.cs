using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyJoy.Backend.Migrations
{
    /// <inheritdoc />
    public partial class DodaniekolumnyIsUrlDoTabeliAtrakcji : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUrl",
                table: "Attractions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUrl",
                table: "Attractions");
        }
    }
}
