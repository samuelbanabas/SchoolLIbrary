using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolLIbrary.Migrations
{
    /// <inheritdoc />
    public partial class addedcheckoutcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e387178-52bc-411f-9ea8-ce784d8895da");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d511771-aab8-4910-b808-309ae5a2129f");

            migrationBuilder.AddColumn<string>(
                name: "CheckoutCode",
                table: "Checkouts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "09a6a78b-5d3f-4124-a09f-2133e7fa281e", null, "Admin", "ADMIN" },
                    { "69c2c1a2-1351-405a-943e-65ef4a9a5cc2", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09a6a78b-5d3f-4124-a09f-2133e7fa281e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69c2c1a2-1351-405a-943e-65ef4a9a5cc2");

            migrationBuilder.DropColumn(
                name: "CheckoutCode",
                table: "Checkouts");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5e387178-52bc-411f-9ea8-ce784d8895da", null, "Admin", "ADMIN" },
                    { "9d511771-aab8-4910-b808-309ae5a2129f", null, "User", "USER" }
                });
        }
    }
}
