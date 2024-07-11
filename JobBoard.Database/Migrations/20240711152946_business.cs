using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Database.Migrations
{
    /// <inheritdoc />
    public partial class business : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BusinessId",
                table: "JobOffers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_BusinessId",
                table: "JobOffers",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_Businesses_BusinessId",
                table: "JobOffers",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_Businesses_BusinessId",
                table: "JobOffers");

            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_BusinessId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "JobOffers");
        }
    }
}
