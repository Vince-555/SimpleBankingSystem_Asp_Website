using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleBankingSystem.Data.Migrations
{
    public partial class PhotoUrlFieldAddedToNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "News");
        }
    }
}
