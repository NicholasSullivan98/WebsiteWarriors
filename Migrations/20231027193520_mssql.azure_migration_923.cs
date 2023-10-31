using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject.Migrations
{
    /// <inheritdoc />
    public partial class mssqlazure_migration_923 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Students_ParentAccountID",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "ParentAccountID",
                table: "Accounts",
                newName: "StudentID");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_ParentAccountID",
                table: "Accounts",
                newName: "IX_Accounts_StudentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Students_StudentID",
                table: "Accounts",
                column: "StudentID",
                principalTable: "Students",
                principalColumn: "StudentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Students_StudentID",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "StudentID",
                table: "Accounts",
                newName: "ParentAccountID");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_StudentID",
                table: "Accounts",
                newName: "IX_Accounts_ParentAccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Students_ParentAccountID",
                table: "Accounts",
                column: "ParentAccountID",
                principalTable: "Students",
                principalColumn: "StudentID");
        }
    }
}
