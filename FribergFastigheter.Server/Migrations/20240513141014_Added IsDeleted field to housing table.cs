using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergFastigheter.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsDeletedfieldtohousingtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Housings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Housings");
        }
    }
}
