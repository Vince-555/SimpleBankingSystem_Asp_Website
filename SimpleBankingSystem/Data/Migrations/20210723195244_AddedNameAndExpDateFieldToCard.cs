using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleBankingSystem.Data.Migrations
{
    public partial class AddedNameAndExpDateFieldToCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardName",
                table: "Cards",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpDate",
                table: "Cards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardName",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ExpDate",
                table: "Cards");
        }
    }
}
