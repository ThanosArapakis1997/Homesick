using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homesick.Services.ListingAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Houses_Listings_ListingId",
                table: "Houses");

            migrationBuilder.DropIndex(
                name: "IX_Houses_ListingId",
                table: "Houses");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_HouseId",
                table: "Listings",
                column: "HouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Houses_HouseId",
                table: "Listings",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "HouseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Houses_HouseId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_HouseId",
                table: "Listings");

            migrationBuilder.CreateIndex(
                name: "IX_Houses_ListingId",
                table: "Houses",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Houses_Listings_ListingId",
                table: "Houses",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "ListingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
