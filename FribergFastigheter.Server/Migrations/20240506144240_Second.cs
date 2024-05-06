using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergFastigheter.Server.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "cda42790-efce-43b0-b569-41648d6c8e82", 0, "745c112a-ef41-4d8b-beed-8265ffcc1504", "kalle@ankeborg.com", true, "Kalle", "Anka", false, null, "kalle@ankeborg.com", "kalle@ankeborg.com", "AQAAAAIAAYagAAAAEDfQbvmzxHhbJmx5Q2zoEOAzVZ/6MEFaxsqi/uLy0K8RqVZ2oTXLtoF6iAaaksJpGw==", null, false, "24bf3030-75ac-462e-907d-7e4d1e0c11fd", false, "kalle@ankeborg.com" });

            migrationBuilder.InsertData(
                table: "BrokerFirms",
                columns: new[] { "BrokerFirmId", "Description", "LogotypeImageId", "Name" },
                values: new object[] { 1, "", null, "Ankeborg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "cda42790-efce-43b0-b569-41648d6c8e82");

            migrationBuilder.DeleteData(
                table: "BrokerFirms",
                keyColumn: "BrokerFirmId",
                keyValue: 1);
        }
    }
}
