using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyJoy.Backend.Migrations
{
    /// <inheritdoc />
    public partial class UsuniecieZbednejKolumnyWTabeliLokacji : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Street2",
                table: "Locations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Street2",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
