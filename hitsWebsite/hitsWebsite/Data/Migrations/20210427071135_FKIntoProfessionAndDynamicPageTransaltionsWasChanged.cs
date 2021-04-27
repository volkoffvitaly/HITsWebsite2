using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hitsWebsite.Data.Migrations
{
    public partial class FKIntoProfessionAndDynamicPageTransaltionsWasChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicPageTranslations_DynamicPages_DynamicPageId",
                table: "DynamicPageTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionTranslations_Professions_ProfessionId",
                table: "ProfessionTranslations");

            migrationBuilder.DropColumn(
                name: "Profession_Id",
                table: "ProfessionTranslations");

            migrationBuilder.DropColumn(
                name: "DynamicPage_Id",
                table: "DynamicPageTranslations");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProfessionId",
                table: "ProfessionTranslations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DynamicPageId",
                table: "DynamicPageTranslations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicPageTranslations_DynamicPages_DynamicPageId",
                table: "DynamicPageTranslations",
                column: "DynamicPageId",
                principalTable: "DynamicPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionTranslations_Professions_ProfessionId",
                table: "ProfessionTranslations",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicPageTranslations_DynamicPages_DynamicPageId",
                table: "DynamicPageTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfessionTranslations_Professions_ProfessionId",
                table: "ProfessionTranslations");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProfessionId",
                table: "ProfessionTranslations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "Profession_Id",
                table: "ProfessionTranslations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "DynamicPageId",
                table: "DynamicPageTranslations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "DynamicPage_Id",
                table: "DynamicPageTranslations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicPageTranslations_DynamicPages_DynamicPageId",
                table: "DynamicPageTranslations",
                column: "DynamicPageId",
                principalTable: "DynamicPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfessionTranslations_Professions_ProfessionId",
                table: "ProfessionTranslations",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
