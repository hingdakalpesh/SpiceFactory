using Microsoft.EntityFrameworkCore.Migrations;

namespace SpiceFactory.Data.Migrations
{
    public partial class AddDescriptionToMenuItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MenuItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "MenuItems");
        }
    }
}
