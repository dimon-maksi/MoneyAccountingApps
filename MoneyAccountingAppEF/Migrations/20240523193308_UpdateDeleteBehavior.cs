using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyAccountingAppEF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Expenses__Accoun__412EB0B6",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK__Income__AccountI__3F466844",
                table: "Income");

            migrationBuilder.DropForeignKey(
                name: "FK__Savings__Account__4316F928",
                table: "Savings");

            migrationBuilder.AddForeignKey(
                name: "FK__Expenses__Accoun__412EB0B6",
                table: "Expenses",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Income__AccountI__3F466844",
                table: "Income",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Savings__Account__4316F928",
                table: "Savings",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Expenses__Accoun__412EB0B6",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK__Income__AccountI__3F466844",
                table: "Income");

            migrationBuilder.DropForeignKey(
                name: "FK__Savings__Account__4316F928",
                table: "Savings");

            migrationBuilder.AddForeignKey(
                name: "FK__Expenses__Accoun__412EB0B6",
                table: "Expenses",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Income__AccountI__3F466844",
                table: "Income",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK__Savings__Account__4316F928",
                table: "Savings",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
