using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergFastigheterApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedfieldstotablesBrokersandBrokerFirms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Brokers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Brokers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProfileImageImageId",
                table: "Brokers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "BrokerFirms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LogotypeImageId",
                table: "BrokerFirms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brokers_ProfileImageImageId",
                table: "Brokers",
                column: "ProfileImageImageId");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerFirms_LogotypeImageId",
                table: "BrokerFirms",
                column: "LogotypeImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrokerFirms_Image_LogotypeImageId",
                table: "BrokerFirms",
                column: "LogotypeImageId",
                principalTable: "Image",
                principalColumn: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Brokers_Image_ProfileImageImageId",
                table: "Brokers",
                column: "ProfileImageImageId",
                principalTable: "Image",
                principalColumn: "ImageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrokerFirms_Image_LogotypeImageId",
                table: "BrokerFirms");

            migrationBuilder.DropForeignKey(
                name: "FK_Brokers_Image_ProfileImageImageId",
                table: "Brokers");

            migrationBuilder.DropIndex(
                name: "IX_Brokers_ProfileImageImageId",
                table: "Brokers");

            migrationBuilder.DropIndex(
                name: "IX_BrokerFirms_LogotypeImageId",
                table: "BrokerFirms");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Brokers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Brokers");

            migrationBuilder.DropColumn(
                name: "ProfileImageImageId",
                table: "Brokers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "BrokerFirms");

            migrationBuilder.DropColumn(
                name: "LogotypeImageId",
                table: "BrokerFirms");
        }
    }
}
