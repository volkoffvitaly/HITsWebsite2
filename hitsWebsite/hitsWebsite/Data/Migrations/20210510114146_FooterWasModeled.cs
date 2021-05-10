using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hitsWebsite.Data.Migrations
{
    public partial class FooterWasModeled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FooterId",
                table: "AchievementTranslations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Footers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Footers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FooterTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FooterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FooterTranslations_Footers_FooterId",
                        column: x => x.FooterId,
                        principalTable: "Footers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AchievementTranslations_FooterId",
                table: "AchievementTranslations",
                column: "FooterId");

            migrationBuilder.CreateIndex(
                name: "IX_FooterTranslations_FooterId",
                table: "FooterTranslations",
                column: "FooterId");

            migrationBuilder.AddForeignKey(
                name: "FK_AchievementTranslations_Footers_FooterId",
                table: "AchievementTranslations",
                column: "FooterId",
                principalTable: "Footers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AchievementTranslations_Footers_FooterId",
                table: "AchievementTranslations");

            migrationBuilder.DropTable(
                name: "FooterTranslations");

            migrationBuilder.DropTable(
                name: "Footers");

            migrationBuilder.DropIndex(
                name: "IX_AchievementTranslations_FooterId",
                table: "AchievementTranslations");

            migrationBuilder.DropColumn(
                name: "FooterId",
                table: "AchievementTranslations");
        }
    }
}
