using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class AddGarages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Garages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PricePerHour = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Garages", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Garages",
                columns: new[] { "Id", "Address", "DateCreated", "Description", "ImageUrl", "IsActive", "Latitude", "Longitude", "OwnerId", "PricePerHour", "Title" },
                values: new object[,]
                {
                    { new Guid("aaaa1111-0000-0000-0000-000000000001"), "123 Main St, Metropolis", new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Secure and covered parking in the city center. Close to everything.", "https://example.com/garage1.jpg", true, 40.7128f, -74.006f, new Guid("11111111-aaaa-bbbb-cccc-111111111111"), 15.00m, "Downtown Covered Parking" },
                    { new Guid("aaaa1111-0000-0000-0000-000000000002"), "456 Elm St, Uptown", new DateTime(2024, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "A quiet driveway spot available during business hours.", "https://example.com/garage2.jpg", true, 34.0522f, -118.2437f, new Guid("22222222-bbbb-cccc-dddd-222222222222"), 10.00m, "Private Driveway Spot" },
                    { new Guid("aaaa1111-0000-0000-0000-000000000003"), "789 Oak Ave, Midtown", new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Underground parking with gate access and cameras.", null, false, 37.7749f, -122.4194f, new Guid("33333333-cccc-dddd-eeee-333333333333"), 18.50m, "Underground Parking Lot" },
                    { new Guid("aaaa1111-0000-0000-0000-000000000004"), "101 Central Blvd, Station City", new DateTime(2024, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Perfect for commuters — safe parking right next to the train.", "https://example.com/garage4.jpg", true, 41.8781f, -87.6298f, new Guid("44444444-dddd-eeee-ffff-444444444444"), 12.00m, "Garage Near Train Station" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Garages");
        }
    }
}
