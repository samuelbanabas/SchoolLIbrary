using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolLIbrary.Migrations
{
    /// <inheritdoc />
    public partial class BorrowedBooksAddedInLibraryUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dc626fce-d859-4703-b8ab-1bea1e8e3260");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e72f2b33-20af-4058-ae2f-a44a20369127");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5e387178-52bc-411f-9ea8-ce784d8895da", null, "Admin", "ADMIN" },
                    { "9d511771-aab8-4910-b808-309ae5a2129f", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5e387178-52bc-411f-9ea8-ce784d8895da");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9d511771-aab8-4910-b808-309ae5a2129f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "dc626fce-d859-4703-b8ab-1bea1e8e3260", null, "User", "USER" },
                    { "e72f2b33-20af-4058-ae2f-a44a20369127", null, "Admin", "ADMIN" }
                });
        }
    }
}
