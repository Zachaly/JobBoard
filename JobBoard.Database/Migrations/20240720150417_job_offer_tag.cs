using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Database.Migrations
{
    /// <inheritdoc />
    public partial class job_offer_tag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobOfferTags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfferId = table.Column<long>(type: "bigint", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOfferTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobOfferTags_JobOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "JobOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferTags_OfferId",
                table: "JobOfferTags",
                column: "OfferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobOfferTags");
        }
    }
}
