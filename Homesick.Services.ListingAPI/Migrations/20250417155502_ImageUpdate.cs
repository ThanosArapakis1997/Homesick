using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homesick.Services.ListingAPI.Migrations
{
    /// <inheritdoc />
    public partial class ImageUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HouseImages_Houses_HouseId",
                table: "HouseImages");

            migrationBuilder.DropIndex(
                name: "IX_Listings_HouseId",
                table: "Listings");

            migrationBuilder.AlterColumn<int>(
                name: "HouseId",
                table: "HouseImages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listings_HouseId",
                table: "Listings",
                column: "HouseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HouseImages_Houses_HouseId",
                table: "HouseImages",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "HouseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HouseImages_Houses_HouseId",
                table: "HouseImages");

            migrationBuilder.DropIndex(
                name: "IX_Listings_HouseId",
                table: "Listings");

            migrationBuilder.AlterColumn<int>(
                name: "HouseId",
                table: "HouseImages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_HouseId",
                table: "Listings",
                column: "HouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_HouseImages_Houses_HouseId",
                table: "HouseImages",
                column: "HouseId",
                principalTable: "Houses",
                principalColumn: "HouseId");
        }
    }
}
