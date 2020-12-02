using Microsoft.EntityFrameworkCore.Migrations;

namespace PracaInzynierska.Migrations
{
    public partial class update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentPr",
                table: "Exercises",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Exercises",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentPr",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Descritpion",
                table: "Exercises");
        }
    }
}
