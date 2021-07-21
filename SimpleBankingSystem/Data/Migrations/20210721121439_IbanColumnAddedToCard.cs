using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleBankingSystem.Data.Migrations
{
    public partial class IbanColumnAddedToCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Iban",
                table: "BankAccounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iban",
                table: "BankAccounts");
        }
    }
}
