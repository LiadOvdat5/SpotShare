using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAvailabilitySlotTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_availabilitySlots",
                table: "availabilitySlots");

            migrationBuilder.RenameTable(
                name: "availabilitySlots",
                newName: "AvailabilitySlots");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvailabilitySlots",
                table: "AvailabilitySlots",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AvailabilitySlots",
                table: "AvailabilitySlots");

            migrationBuilder.RenameTable(
                name: "AvailabilitySlots",
                newName: "availabilitySlots");

            migrationBuilder.AddPrimaryKey(
                name: "PK_availabilitySlots",
                table: "availabilitySlots",
                column: "Id");
        }
    }
}
