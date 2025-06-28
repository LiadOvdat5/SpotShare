using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAvailabilitySlotModelWithoutDayofWeek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "AvailabilitySlots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "AvailabilitySlots",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
