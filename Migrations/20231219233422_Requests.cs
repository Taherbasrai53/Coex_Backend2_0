using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COeX_India1._2.Migrations
{
    /// <inheritdoc />
    public partial class Requests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SidingId = table.Column<int>(type: "int", nullable: false),
                    FrieghtAmount = table.Column<double>(type: "float", nullable: false),
                    RakesRequired = table.Column<int>(type: "int", nullable: false),
                    RequiredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlacedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsertedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");
        }
    }
}
