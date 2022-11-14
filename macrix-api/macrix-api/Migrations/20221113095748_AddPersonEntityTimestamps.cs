using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace macrix_api.Migrations
{
    public partial class AddPersonEntityTimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTimestamp",
                table: "peopleEntities",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateTimestamp",
                table: "peopleEntities",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTimestamp",
                table: "peopleEntities");

            migrationBuilder.DropColumn(
                name: "LastUpdateTimestamp",
                table: "peopleEntities");
        }
    }
}
