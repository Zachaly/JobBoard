using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Database.Migrations
{
    /// <inheritdoc />
    public partial class refresh_tokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminAccountRefreshTokens",
                columns: table => new
                {
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminAccountRefreshTokens", x => x.Token);
                    table.ForeignKey(
                        name: "FK_AdminAccountRefreshTokens_AdminAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AdminAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyAccountRefreshTokens",
                columns: table => new
                {
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAccountRefreshTokens", x => x.Token);
                    table.ForeignKey(
                        name: "FK_CompanyAccountRefreshTokens_CompanyAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "CompanyAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAccountRefreshTokens",
                columns: table => new
                {
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAccountRefreshTokens", x => x.Token);
                    table.ForeignKey(
                        name: "FK_EmployeeAccountRefreshTokens_EmployeeAccounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "EmployeeAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminAccountRefreshTokens_AccountId",
                table: "AdminAccountRefreshTokens",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAccountRefreshTokens_AccountId",
                table: "CompanyAccountRefreshTokens",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAccountRefreshTokens_AccountId",
                table: "EmployeeAccountRefreshTokens",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminAccountRefreshTokens");

            migrationBuilder.DropTable(
                name: "CompanyAccountRefreshTokens");

            migrationBuilder.DropTable(
                name: "EmployeeAccountRefreshTokens");
        }
    }
}
