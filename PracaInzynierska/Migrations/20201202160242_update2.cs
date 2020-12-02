using Microsoft.EntityFrameworkCore.Migrations;

namespace PracaInzynierska.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descritpion",
                table: "Exercises");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Exercises",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Exercises");

            migrationBuilder.AddColumn<string>(
                name: "Descritpion",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
