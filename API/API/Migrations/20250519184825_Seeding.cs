using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class Seeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateCreated", "Email", "Name", "Password", "Phone", "RoleId" },
                values: new object[,]
                {
                    { new Guid("11111111-aaaa-bbbb-cccc-111111111111"), new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "alice@example.com", "Alice Johnson", "hashed_password_1", "555-1234", 1 },
                    { new Guid("22222222-bbbb-cccc-dddd-222222222222"), new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "bob@example.com", "Bob Smith", "hashed_password_2", "555-5678", 2 },
                    { new Guid("33333333-cccc-dddd-eeee-333333333333"), new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "charlie@example.com", "Charlie Evans", "hashed_password_3", null, 1 },
                    { new Guid("44444444-dddd-eeee-ffff-444444444444"), new DateTime(2024, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "diana@example.com", "Diana Carter", "hashed_password_4", "555-8765", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-bbbb-cccc-111111111111"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-cccc-dddd-222222222222"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-cccc-dddd-eeee-333333333333"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-eeee-ffff-444444444444"));
        }
    }
}
