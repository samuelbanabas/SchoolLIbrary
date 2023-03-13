using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolLIbrary.Migrations
{
    /// <inheritdoc />
    public partial class addmigrationUpdatedCheckoutModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_LibraryUsers_UserId",
                table: "Checkouts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "52cfee5a-54a5-42ae-8b16-51e1c12478f6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7357e16a-51fb-465b-ad27-22980f5510bb");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Checkouts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "LibraryUserId",
                table: "Checkouts",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3a6b16ea-6cfb-4558-b6cf-3edf5a209b65", null, "User", "USER" },
                    { "c64a1d2e-6b47-4345-a160-121030470f55", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Checkouts_LibraryUserId",
                table: "Checkouts",
                column: "LibraryUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_AspNetUsers_UserId",
                table: "Checkouts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_LibraryUsers_LibraryUserId",
                table: "Checkouts",
                column: "LibraryUserId",
                principalTable: "LibraryUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_AspNetUsers_UserId",
                table: "Checkouts");

            migrationBuilder.DropForeignKey(
                name: "FK_Checkouts_LibraryUsers_LibraryUserId",
                table: "Checkouts");

            migrationBuilder.DropIndex(
                name: "IX_Checkouts_LibraryUserId",
                table: "Checkouts");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3a6b16ea-6cfb-4558-b6cf-3edf5a209b65");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c64a1d2e-6b47-4345-a160-121030470f55");

            migrationBuilder.DropColumn(
                name: "LibraryUserId",
                table: "Checkouts");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Checkouts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "52cfee5a-54a5-42ae-8b16-51e1c12478f6", null, "User", "USER" },
                    { "7357e16a-51fb-465b-ad27-22980f5510bb", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Checkouts_LibraryUsers_UserId",
                table: "Checkouts",
                column: "UserId",
                principalTable: "LibraryUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
