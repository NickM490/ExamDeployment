using Microsoft.EntityFrameworkCore.Migrations;

namespace Exam.Migrations
{
    public partial class work2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RSVPs_Happenings_HappeningId",
                table: "RSVPs");

            migrationBuilder.DropColumn(
                name: "WeddingId",
                table: "RSVPs");

            migrationBuilder.AlterColumn<int>(
                name: "HappeningId",
                table: "RSVPs",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RSVPs_Happenings_HappeningId",
                table: "RSVPs",
                column: "HappeningId",
                principalTable: "Happenings",
                principalColumn: "HappeningId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RSVPs_Happenings_HappeningId",
                table: "RSVPs");

            migrationBuilder.AlterColumn<int>(
                name: "HappeningId",
                table: "RSVPs",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "WeddingId",
                table: "RSVPs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_RSVPs_Happenings_HappeningId",
                table: "RSVPs",
                column: "HappeningId",
                principalTable: "Happenings",
                principalColumn: "HappeningId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
