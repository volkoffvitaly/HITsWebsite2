using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hitsWebsite.Data.Migrations
{
    public partial class CityFeaturesModeled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CityFeatureId",
                table: "Pictures",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CityFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CityFeatureTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityFeatureId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityFeatureTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityFeatureTranslations_CityFeatures_CityFeatureId",
                        column: x => x.CityFeatureId,
                        principalTable: "CityFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_CityFeatureId",
                table: "Pictures",
                column: "CityFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_CityFeatureTranslations_CityFeatureId",
                table: "CityFeatureTranslations",
                column: "CityFeatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_CityFeatures_CityFeatureId",
                table: "Pictures",
                column: "CityFeatureId",
                principalTable: "CityFeatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_CityFeatures_CityFeatureId",
                table: "Pictures");

            migrationBuilder.DropTable(
                name: "CityFeatureTranslations");

            migrationBuilder.DropTable(
                name: "CityFeatures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_CityFeatureId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "CityFeatureId",
                table: "Pictures");
        }
    }
}
