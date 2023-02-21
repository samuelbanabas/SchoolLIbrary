using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolLIbrary.Migrations
{
    /// <inheritdoc />
    public partial class NewForAddDateCreatedToMaterialModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "34d82238-6788-4b60-a3a5-e078b9dc567a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d54807a-bd72-4bb5-afea-83ca695d0fca");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Materials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "053b559f-c77c-4677-af98-1b4726b862a7", null, "User", "USER" },
                    { "fac3f601-37a6-4a64-bbc0-88774fbbd931", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "053b559f-c77c-4677-af98-1b4726b862a7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fac3f601-37a6-4a64-bbc0-88774fbbd931");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Materials");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "34d82238-6788-4b60-a3a5-e078b9dc567a", null, "Admin", "ADMIN" },
                    { "3d54807a-bd72-4bb5-afea-83ca695d0fca", null, "User", "USER" }
                });
        }
    }
}
