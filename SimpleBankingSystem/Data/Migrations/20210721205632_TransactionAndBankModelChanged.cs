using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleBankingSystem.Data.Migrations
{
    public partial class TransactionAndBankModelChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_ReceiverId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_SenderId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_BankAccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_BankAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "BankAccountId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Transactions",
                newName: "SenderBankAccId");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Transactions",
                newName: "ReceiverBankAccId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SenderId",
                table: "Transactions",
                newName: "IX_Transactions_SenderBankAccId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ReceiverId",
                table: "Transactions",
                newName: "IX_Transactions_ReceiverBankAccId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_ReceiverBankAccId",
                table: "Transactions",
                column: "ReceiverBankAccId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_SenderBankAccId",
                table: "Transactions",
                column: "SenderBankAccId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_ReceiverBankAccId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_BankAccounts_SenderBankAccId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SenderBankAccId",
                table: "Transactions",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "ReceiverBankAccId",
                table: "Transactions",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SenderBankAccId",
                table: "Transactions",
                newName: "IX_Transactions_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ReceiverBankAccId",
                table: "Transactions",
                newName: "IX_Transactions_ReceiverId");

            migrationBuilder.AddColumn<string>(
                name: "BankAccountId",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BankAccountId",
                table: "Transactions",
                column: "BankAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_ReceiverId",
                table: "Transactions",
                column: "ReceiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_SenderId",
                table: "Transactions",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_BankAccounts_BankAccountId",
                table: "Transactions",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
