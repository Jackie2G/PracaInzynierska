using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PracaInzynierska.Migrations
{
    public partial class changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrainingHistory_ExercisesID",
                table: "TrainingHistory");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "TrainingHistory",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistory_ExercisesID",
                table: "TrainingHistory",
                column: "ExercisesID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrainingHistory_ExercisesID",
                table: "TrainingHistory");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "TrainingHistory",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistory_ExercisesID",
                table: "TrainingHistory",
                column: "ExercisesID");
        }
    }
}
