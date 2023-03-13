using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolLIbrary.Migrations
{
    /// <inheritdoc />
    public partial class Newroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92e0ff8c-0432-4a93-85a2-c398b40a1175");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e87fb2e4-2d7a-4ef5-bca4-70558cfa0e69");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "30b5426e-1f72-4e09-b68c-a68c2c16c8e4", null, "Student", "STUDENT" },
                    { "5346dab5-86b5-4c4f-a5ab-1554df16a730", null, "Lecturer", "LECTURER" },
                    { "cc978e3e-d2e8-4ed2-bcc0-f4a5e6dc3519", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "30b5426e-1f72-4e09-b68c-a68c2c16c8e4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5346dab5-86b5-4c4f-a5ab-1554df16a730");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc978e3e-d2e8-4ed2-bcc0-f4a5e6dc3519");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "92e0ff8c-0432-4a93-85a2-c398b40a1175", null, "User", "USER" },
                    { "e87fb2e4-2d7a-4ef5-bca4-70558cfa0e69", null, "Admin", "ADMIN" }
                });
        }
    }
}
