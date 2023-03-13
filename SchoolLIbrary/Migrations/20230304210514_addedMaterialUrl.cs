using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolLIbrary.Migrations
{
    /// <inheritdoc />
    public partial class addedMaterialUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "09a6a78b-5d3f-4124-a09f-2133e7fa281e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69c2c1a2-1351-405a-943e-65ef4a9a5cc2");

            migrationBuilder.AddColumn<string>(
                name: "MaterialUrl",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8ad31256-13a7-40e0-9749-306d0e610cab", null, "User", "USER" },
                    { "ed44108c-6196-4dbf-81de-8ac5704ca6a0", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ad31256-13a7-40e0-9749-306d0e610cab");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed44108c-6196-4dbf-81de-8ac5704ca6a0");

            migrationBuilder.DropColumn(
                name: "MaterialUrl",
                table: "Materials");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "09a6a78b-5d3f-4124-a09f-2133e7fa281e", null, "Admin", "ADMIN" },
                    { "69c2c1a2-1351-405a-943e-65ef4a9a5cc2", null, "User", "USER" }
                });
        }
    }
}
