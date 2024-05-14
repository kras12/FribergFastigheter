using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergFastigheter.Server.Migrations
{
    /// <inheritdoc />
    public partial class Deletedbrokerfirmfieldinhousingtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Housings_BrokerFirms_BrokerFirmId",
                table: "Housings");

            migrationBuilder.DropIndex(
                name: "IX_Housings_BrokerFirmId",
                table: "Housings");

            migrationBuilder.DropColumn(
                name: "BrokerFirmId",
                table: "Housings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BrokerFirmId",
                table: "Housings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Housings_BrokerFirmId",
                table: "Housings",
                column: "BrokerFirmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Housings_BrokerFirms_BrokerFirmId",
                table: "Housings",
                column: "BrokerFirmId",
                principalTable: "BrokerFirms",
                principalColumn: "BrokerFirmId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
