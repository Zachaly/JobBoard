using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_job_offer_salary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxSalary",
                table: "JobOffers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinSalary",
                table: "JobOffers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalaryType",
                table: "JobOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxSalary",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "MinSalary",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "SalaryType",
                table: "JobOffers");
        }
    }
}
