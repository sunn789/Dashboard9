using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modicom.Models.Migrations
{
    /// <inheritdoc />
    public partial class EditContactUsMOdel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "ContactUs",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "ContactUs");
        }
    }
}
