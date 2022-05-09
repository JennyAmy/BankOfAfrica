using Microsoft.EntityFrameworkCore.Migrations;

namespace BankOfAfricaAPI.Migrations
{
    public partial class IsValidatedaddedtousertable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValidated",
                table: "AppUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValidated",
                table: "AppUsers");
        }
    }
}
