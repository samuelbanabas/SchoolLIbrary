using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolLIbrary.Migrations
{
    /// <inheritdoc />
    public partial class AddDateCreatedToMaterialModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "898a5ad9-cda5-4b38-8f95-04a92639b9a9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "89f25bb7-3404-4ba5-805c-5273cbc9a01f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "34d82238-6788-4b60-a3a5-e078b9dc567a", null, "Admin", "ADMIN" },
                    { "3d54807a-bd72-4bb5-afea-83ca695d0fca", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "34d82238-6788-4b60-a3a5-e078b9dc567a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d54807a-bd72-4bb5-afea-83ca695d0fca");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "898a5ad9-cda5-4b38-8f95-04a92639b9a9", null, "User", "USER" },
                    { "89f25bb7-3404-4ba5-805c-5273cbc9a01f", null, "Admin", "ADMIN" }
                });
        }
    }
}
