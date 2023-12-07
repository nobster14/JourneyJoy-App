using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyJoy.Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTabelekAtrakcjiILokalizacji : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StreetName",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "Y",
                table: "Locations",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "X",
                table: "Locations",
                newName: "Latitude");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Postalcode",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street1",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street2",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationType",
                table: "Attractions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OpenHours",
                table: "Attractions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Prices",
                table: "Attractions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "TimeNeeded",
                table: "Attractions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Postalcode",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Street1",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Street2",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LocationType",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "OpenHours",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "Prices",
                table: "Attractions");

            migrationBuilder.DropColumn(
                name: "TimeNeeded",
                table: "Attractions");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Locations",
                newName: "Y");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Locations",
                newName: "X");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Locations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StreetName",
                table: "Locations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
