using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentFirstName",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "StudentLastName",
                table: "Accounts",
                newName: "OldPassword");

            migrationBuilder.AddColumn<int>(
                name: "StudentID",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentLastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_StudentID",
                table: "Accounts",
                column: "StudentID");

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

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_StudentID",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "StudentID",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "OldPassword",
                table: "Accounts",
                newName: "StudentLastName");

            migrationBuilder.AddColumn<string>(
                name: "StudentFirstName",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
