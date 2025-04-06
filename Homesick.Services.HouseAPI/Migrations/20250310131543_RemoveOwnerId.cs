using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homesick.Services.HouseAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOwnerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Houses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Houses",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
