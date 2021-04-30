using Microsoft.EntityFrameworkCore.Migrations;

namespace hitsWebsite.Data.Migrations
{
    public partial class RestrictDeletingHumanWhenPictureDeleting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Humans_Pictures_PictureId",
                table: "Humans");

            migrationBuilder.DropIndex(
                name: "IX_Humans_PictureId",
                table: "Humans");

            migrationBuilder.CreateIndex(
                name: "IX_Humans_PictureId",
                table: "Humans",
                column: "PictureId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Humans_Pictures_PictureId",
                table: "Humans",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Humans_Pictures_PictureId",
                table: "Humans");

            migrationBuilder.DropIndex(
                name: "IX_Humans_PictureId",
                table: "Humans");

            migrationBuilder.CreateIndex(
                name: "IX_Humans_PictureId",
                table: "Humans",
                column: "PictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Humans_Pictures_PictureId",
                table: "Humans",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
