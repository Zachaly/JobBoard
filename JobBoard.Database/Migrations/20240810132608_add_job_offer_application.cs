using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Database.Migrations
{
    /// <inheritdoc />
    public partial class add_job_offer_application : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResumeFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    OfferId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Applications_EmployeeAccounts_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EmployeeAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applications_JobOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "JobOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_EmployeeId",
                table: "Applications",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_OfferId",
                table: "Applications",
                column: "OfferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Applications");
        }
    }
}
