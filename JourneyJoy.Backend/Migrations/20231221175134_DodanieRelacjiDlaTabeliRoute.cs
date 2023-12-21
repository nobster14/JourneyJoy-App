using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JourneyJoy.Backend.Migrations
{
    /// <inheritdoc />
    public partial class DodanieRelacjiDlaTabeliRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Routes_RouteId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_RouteId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "TripLength",
                table: "Routes");

            migrationBuilder.RenameColumn(
                name: "DayLength",
                table: "Routes",
                newName: "StartDay");

            migrationBuilder.AddColumn<string>(
                name: "SerializedAttractionsIds",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerializedAttractionsIds",
                table: "Routes");

            migrationBuilder.RenameColumn(
                name: "StartDay",
                table: "Routes",
                newName: "DayLength");

            migrationBuilder.AddColumn<Guid>(
                name: "RouteId",
                table: "Routes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TripLength",
                table: "Routes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_Routes_RouteId",
                table: "Routes",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Routes_RouteId",
                table: "Routes",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id");
        }
    }
}
