using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject.Migrations
{
    /// <inheritdoc />
    public partial class AddConfirmationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerified",
                table: "Accounts");

            migrationBuilder.CreateTable(
                name: "Confirmations",
                columns: table => new
                {
                    ConfirmationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Confirmations", x => x.ConfirmationID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Confirmations");

            migrationBuilder.AddColumn<bool>(
                name: "EmailVerified",
                table: "Accounts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
