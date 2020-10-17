using Microsoft.EntityFrameworkCore.Migrations;

namespace Lares.Migrations
{
    public partial class defaultprop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SelectedPropertyId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedPropertyId",
                table: "AspNetUsers");
        }
    }
}
