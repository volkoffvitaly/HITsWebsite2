using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hitsWebsite.Data.Migrations
{
    public partial class DormitoryWasModeled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DormitoryId",
                table: "Pictures",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Dormitories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dormitories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DormitoryTranslations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DormitoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DormitoryTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DormitoryTranslations_Dormitories_DormitoryId",
                        column: x => x.DormitoryId,
                        principalTable: "Dormitories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_DormitoryId",
                table: "Pictures",
                column: "DormitoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DormitoryTranslations_DormitoryId",
                table: "DormitoryTranslations",
                column: "DormitoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Dormitories_DormitoryId",
                table: "Pictures",
                column: "DormitoryId",
                principalTable: "Dormitories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Dormitories_DormitoryId",
                table: "Pictures");

            migrationBuilder.DropTable(
                name: "DormitoryTranslations");

            migrationBuilder.DropTable(
                name: "Dormitories");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_DormitoryId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "DormitoryId",
                table: "Pictures");
        }
    }
}
