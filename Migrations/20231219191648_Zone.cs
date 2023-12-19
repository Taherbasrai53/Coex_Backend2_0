using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace COeX_India1._2.Migrations
{
    /// <inheritdoc />
    public partial class Zone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableRakes",
                table: "Sidings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Zone",
                table: "Sidings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableRakes",
                table: "Sidings");

            migrationBuilder.DropColumn(
                name: "Zone",
                table: "Sidings");
        }
    }
}
