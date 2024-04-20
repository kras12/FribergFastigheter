using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergFastigheterApi.Migrations
{
    /// <inheritdoc />
    public partial class Addeddescriptionfieldttobrokerstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Brokers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Brokers");
        }
    }
}
