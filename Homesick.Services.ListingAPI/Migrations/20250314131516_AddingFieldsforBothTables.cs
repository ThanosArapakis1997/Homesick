using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homesick.Services.ListingAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddingFieldsforBothTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Houses",
                newName: "PropertyType");

            migrationBuilder.RenameColumn(
                name: "ImageLocalPath",
                table: "Houses",
                newName: "Imagepaths");

            migrationBuilder.AddColumn<string>(
                name: "ListingType",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HouseType",
                table: "Houses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListingType",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "HouseType",
                table: "Houses");

            migrationBuilder.RenameColumn(
                name: "PropertyType",
                table: "Houses",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "Imagepaths",
                table: "Houses",
                newName: "ImageLocalPath");
        }
    }
}
