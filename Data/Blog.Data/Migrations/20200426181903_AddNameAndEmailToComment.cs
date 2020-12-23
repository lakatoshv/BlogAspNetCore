using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.Migrations
{
    public partial class AddNameAndEmailToComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Comments");
        }
    }
}
