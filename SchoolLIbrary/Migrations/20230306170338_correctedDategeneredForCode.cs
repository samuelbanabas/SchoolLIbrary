using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolLIbrary.Migrations
{
    /// <inheritdoc />
    public partial class correctedDategeneredForCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6817d8d8-755c-476d-bd1b-0945ac7087e8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d35d79e8-662a-41a1-becd-27f18443b1ca");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateGenerated",
                table: "Regcodes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "92e0ff8c-0432-4a93-85a2-c398b40a1175", null, "User", "USER" },
                    { "e87fb2e4-2d7a-4ef5-bca4-70558cfa0e69", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "92e0ff8c-0432-4a93-85a2-c398b40a1175");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e87fb2e4-2d7a-4ef5-bca4-70558cfa0e69");

            migrationBuilder.DropColumn(
                name: "DateGenerated",
                table: "Regcodes");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6817d8d8-755c-476d-bd1b-0945ac7087e8", null, "User", "USER" },
                    { "d35d79e8-662a-41a1-becd-27f18443b1ca", null, "Admin", "ADMIN" }
                });
        }
    }
}
