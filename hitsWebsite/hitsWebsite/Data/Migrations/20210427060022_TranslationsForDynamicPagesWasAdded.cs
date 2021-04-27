using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hitsWebsite.Data.Migrations
{
    public partial class TranslationsForDynamicPagesWasAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "DynamicPages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DynamicPages");

            migrationBuilder.CreateTable(
                name: "DynamicPageTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DynamicPage_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DynamicPageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicPageTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicPageTranslations_DynamicPages_DynamicPageId",
                        column: x => x.DynamicPageId,
                        principalTable: "DynamicPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DynamicPageTranslations_DynamicPageId",
                table: "DynamicPageTranslations",
                column: "DynamicPageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DynamicPageTranslations");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "DynamicPages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DynamicPages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
