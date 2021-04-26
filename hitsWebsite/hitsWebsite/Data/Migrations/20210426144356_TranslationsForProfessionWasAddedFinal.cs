using Microsoft.EntityFrameworkCore.Migrations;

namespace hitsWebsite.Data.Migrations
{
    public partial class TranslationsForProfessionWasAddedFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionTranslation_Professions_ProfessionId",
                table: "ProfessionTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfessionTranslation",
                table: "ProfessionTranslation");

            migrationBuilder.RenameTable(
                name: "ProfessionTranslation",
                newName: "ProfessionTranslations");

            migrationBuilder.RenameIndex(
                name: "IX_ProfessionTranslation_ProfessionId",
                table: "ProfessionTranslations",
                newName: "IX_ProfessionTranslations_ProfessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfessionTranslations",
                table: "ProfessionTranslations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionTranslations_Professions_ProfessionId",
                table: "ProfessionTranslations",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionTranslations_Professions_ProfessionId",
                table: "ProfessionTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfessionTranslations",
                table: "ProfessionTranslations");

            migrationBuilder.RenameTable(
                name: "ProfessionTranslations",
                newName: "ProfessionTranslation");

            migrationBuilder.RenameIndex(
                name: "IX_ProfessionTranslations_ProfessionId",
                table: "ProfessionTranslation",
                newName: "IX_ProfessionTranslation_ProfessionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfessionTranslation",
                table: "ProfessionTranslation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionTranslation_Professions_ProfessionId",
                table: "ProfessionTranslation",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
