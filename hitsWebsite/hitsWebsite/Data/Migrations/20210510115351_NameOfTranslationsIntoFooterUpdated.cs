using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hitsWebsite.Data.Migrations
{
    public partial class NameOfTranslationsIntoFooterUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AchievementTranslations_Footers_FooterId",
                table: "AchievementTranslations");

            migrationBuilder.DropIndex(
                name: "IX_AchievementTranslations_FooterId",
                table: "AchievementTranslations");

            migrationBuilder.DropColumn(
                name: "FooterId",
                table: "AchievementTranslations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FooterId",
                table: "AchievementTranslations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AchievementTranslations_FooterId",
                table: "AchievementTranslations",
                column: "FooterId");

            migrationBuilder.AddForeignKey(
                name: "FK_AchievementTranslations_Footers_FooterId",
                table: "AchievementTranslations",
                column: "FooterId",
                principalTable: "Footers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
