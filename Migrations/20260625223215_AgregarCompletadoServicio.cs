using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FunerariaApp.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCompletadoServicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completado",
                table: "Servicios",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completado",
                table: "Servicios");
        }
    }
}
