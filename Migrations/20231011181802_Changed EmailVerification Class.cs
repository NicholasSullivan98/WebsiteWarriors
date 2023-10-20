using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject.Migrations
{
    /// <inheritdoc />
    public partial class ChangedEmailVerificationClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentFirstName",
                table: "Confirmations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ParentLastName",
                table: "Confirmations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Confirmations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordConformation",
                table: "Confirmations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Confirmations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentFirstName",
                table: "Confirmations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentLastName",
                table: "Confirmations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentFirstName",
                table: "Confirmations");

            migrationBuilder.DropColumn(
                name: "ParentLastName",
                table: "Confirmations");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Confirmations");

            migrationBuilder.DropColumn(
                name: "PasswordConformation",
                table: "Confirmations");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Confirmations");

            migrationBuilder.DropColumn(
                name: "StudentFirstName",
                table: "Confirmations");

            migrationBuilder.DropColumn(
                name: "StudentLastName",
                table: "Confirmations");
        }
    }
}
