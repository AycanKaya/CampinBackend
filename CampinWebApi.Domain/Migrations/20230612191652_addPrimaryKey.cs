using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampinWebApi.Domain.Migrations
{
    public partial class addPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FavoriteCampsites",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteCampsites",
                table: "FavoriteCampsites",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteCampsites",
                table: "FavoriteCampsites");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FavoriteCampsites");
        }
    }
}
