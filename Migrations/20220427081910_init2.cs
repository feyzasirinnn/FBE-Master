using Microsoft.EntityFrameworkCore.Migrations;

namespace FBE.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Akademik_KadroSicil_No",
                table: "EYKUyeler",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EABD_Id",
                table: "EYKUyeler",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EYKUyeler_Akademik_KadroSicil_No",
                table: "EYKUyeler",
                column: "Akademik_KadroSicil_No");

            migrationBuilder.CreateIndex(
                name: "IX_EYKUyeler_EABD_Id",
                table: "EYKUyeler",
                column: "EABD_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EYKUyeler_Akademik_Kadro_Akademik_KadroSicil_No",
                table: "EYKUyeler",
                column: "Akademik_KadroSicil_No",
                principalTable: "Akademik_Kadro",
                principalColumn: "Sicil_No",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EYKUyeler_EABD_EABD_Id",
                table: "EYKUyeler",
                column: "EABD_Id",
                principalTable: "EABD",
                principalColumn: "EABD_Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EYKUyeler_Akademik_Kadro_Akademik_KadroSicil_No",
                table: "EYKUyeler");

            migrationBuilder.DropForeignKey(
                name: "FK_EYKUyeler_EABD_EABD_Id",
                table: "EYKUyeler");

            migrationBuilder.DropIndex(
                name: "IX_EYKUyeler_Akademik_KadroSicil_No",
                table: "EYKUyeler");

            migrationBuilder.DropIndex(
                name: "IX_EYKUyeler_EABD_Id",
                table: "EYKUyeler");

            migrationBuilder.DropColumn(
                name: "Akademik_KadroSicil_No",
                table: "EYKUyeler");

            migrationBuilder.DropColumn(
                name: "EABD_Id",
                table: "EYKUyeler");
        }
    }
}
