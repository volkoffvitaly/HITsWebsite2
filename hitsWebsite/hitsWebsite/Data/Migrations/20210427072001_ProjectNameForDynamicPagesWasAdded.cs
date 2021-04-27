using Microsoft.EntityFrameworkCore.Migrations;

namespace hitsWebsite.Data.Migrations
{
    public partial class ProjectNameForDynamicPagesWasAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "DynamicPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "DynamicPages");
        }
    }
}
