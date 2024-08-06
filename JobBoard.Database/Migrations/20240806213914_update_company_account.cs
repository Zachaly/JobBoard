using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Database.Migrations
{
    /// <inheritdoc />
    public partial class update_company_account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "CompanyAccounts",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "About",
                table: "CompanyAccounts");
        }
    }
}
