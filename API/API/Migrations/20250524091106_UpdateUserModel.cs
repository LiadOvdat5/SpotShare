using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-bbbb-cccc-111111111111"),
                columns: new[] { "FullName", "PasswordHash", "Username" },
                values: new object[] { "Alice Johnson", "hashed_password_1", "Alice1" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-cccc-dddd-222222222222"),
                columns: new[] { "FullName", "PasswordHash", "Username" },
                values: new object[] { "Bob Smith", "hashed_password_2", "Bob2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-cccc-dddd-eeee-333333333333"),
                columns: new[] { "FullName", "PasswordHash", "Username" },
                values: new object[] { "Charlie Evans", "hashed_password_3", "Charlie3" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-eeee-ffff-444444444444"),
                columns: new[] { "FullName", "PasswordHash", "Username" },
                values: new object[] { "Diana Carter", "hashed_password_4", "Diana4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "Name");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-aaaa-bbbb-cccc-111111111111"),
                columns: new[] { "Name", "Password" },
                values: new object[] { "Alice Johnson", "hashed_password_1" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-bbbb-cccc-dddd-222222222222"),
                columns: new[] { "Name", "Password" },
                values: new object[] { "Bob Smith", "hashed_password_2" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-cccc-dddd-eeee-333333333333"),
                columns: new[] { "Name", "Password" },
                values: new object[] { "Charlie Evans", "hashed_password_3" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44444444-dddd-eeee-ffff-444444444444"),
                columns: new[] { "Name", "Password" },
                values: new object[] { "Diana Carter", "hashed_password_4" });
        }
    }
}
