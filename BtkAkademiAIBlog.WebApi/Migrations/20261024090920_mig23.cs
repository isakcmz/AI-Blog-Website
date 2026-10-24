using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BtkAkademiAIBlog.WebApi.Migrations
{
    public partial class mig23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubFeatureImageUrl",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SubFeatureStatus",
                table: "Articles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubFeatureImageUrl",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "SubFeatureStatus",
                table: "Articles");
        }
    }
}
