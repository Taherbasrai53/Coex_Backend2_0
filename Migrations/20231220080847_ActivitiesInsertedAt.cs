using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COeX_India1._2.Migrations
{
    /// <inheritdoc />
    public partial class ActivitiesInsertedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InsertedAt",
                table: "Activities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsertedAt",
                table: "Activities");
        }
    }
}
