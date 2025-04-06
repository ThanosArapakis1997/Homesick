using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homesick.Services.ListingAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {          

            migrationBuilder.AddColumn<int>(
                name: "ListingId",
                table: "Houses",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Houses_Listings_ListingId",
                table: "Houses");

            migrationBuilder.DropIndex(
                name: "IX_Houses_ListingId",
                table: "Houses");

            migrationBuilder.DropColumn(
                name: "ListingId",
                table: "Houses");
            
        }
    }
}
