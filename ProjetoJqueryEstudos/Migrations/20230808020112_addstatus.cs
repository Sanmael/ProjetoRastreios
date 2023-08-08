using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoJqueryEstudos.Migrations
{
    /// <inheritdoc />
    public partial class addstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NextSearch",
                table: "TrackingCode",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "TrackingCode",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextSearch",
                table: "TrackingCode");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "TrackingCode");
        }
    }
}
