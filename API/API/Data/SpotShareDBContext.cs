using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class SpotShareDBContext(DbContextOptions<SpotShareDBContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Garage> Garages => Set<Garage>();
        public DbSet<AvailabilitySlot> AvailabilitySlots => Set<AvailabilitySlot>();
        public DbSet<Booking> Bookings => Set<Booking>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(

                    new User
                    {
                        Id = Guid.Parse("11111111-aaaa-bbbb-cccc-111111111111"),
                        FullName = "Alice Johnson",
                        Username = "Alice1",
                        PasswordHash = "hashed_password_1", // Normally store hashed passwords
                        Email = "alice@example.com",
                        Phone = "555-1234",
                        DateCreated = new DateTime(2024, 01, 15),
                        Role = "User"
                    },
                    new User
                    {
                        Id = Guid.Parse("22222222-bbbb-cccc-dddd-222222222222"),
                        FullName = "Bob Smith",
                        Username = "Bob2",
                        PasswordHash = "hashed_password_2",
                        Email = "bob@example.com",
                        Phone = "555-5678",
                        DateCreated = new DateTime(2024, 02, 10),
                        Role = "User"
                    },
                    new User
                    {
                        Id = Guid.Parse("33333333-cccc-dddd-eeee-333333333333"),
                        FullName = "Charlie Evans",
                        Username = "Charlie3",
                        PasswordHash = "hashed_password_3",
                        Email = "charlie@example.com",
                        Phone = null,
                        DateCreated = new DateTime(2024, 03, 05),
                        Role = "User"
                    },
                    new User
                    {
                        Id = Guid.Parse("44444444-dddd-eeee-ffff-444444444444"),
                        FullName = "Diana Carter",
                        Username = "Diana4",
                        PasswordHash = "hashed_password_4",
                        Email = "diana@example.com",
                        Phone = "555-8765",
                        DateCreated = new DateTime(2024, 04, 22),
                        Role = "User"
                    }
                );

            modelBuilder.Entity<Garage>()
                .Property(g => g.PricePerHour)
                .HasPrecision(10, 2); // 10 digits total, 2 after the decimal point

            modelBuilder.Entity<Garage>().HasData(
                    new Garage
                    {
                        Id = Guid.Parse("aaaa1111-0000-0000-0000-000000000001"),
                        OwnerId = Guid.Parse("11111111-aaaa-bbbb-cccc-111111111111"), // Alice
                        Title = "Downtown Covered Parking",
                        Description = "Secure and covered parking in the city center. Close to everything.",
                        Address = "123 Main St, Metropolis",
                        Latitude = 40.7128f,
                        Longitude = -74.0060f,
                        ImageUrl = "https://example.com/garage1.jpg",
                        PricePerHour = 15.00m,
                        IsActive = true,
                        DateCreated = new DateTime(2024, 01, 20)
                    },
                    new Garage
                    {
                        Id = Guid.Parse("aaaa1111-0000-0000-0000-000000000002"),
                        OwnerId = Guid.Parse("22222222-bbbb-cccc-dddd-222222222222"), // Bob
                        Title = "Private Driveway Spot",
                        Description = "A quiet driveway spot available during business hours.",
                        Address = "456 Elm St, Uptown",
                        Latitude = 34.0522f,
                        Longitude = -118.2437f,
                        ImageUrl = "https://example.com/garage2.jpg",
                        PricePerHour = 10.00m,
                        IsActive = true,
                        DateCreated = new DateTime(2024, 02, 12)
                    },
                    new Garage
                    {
                        Id = Guid.Parse("aaaa1111-0000-0000-0000-000000000003"),
                        OwnerId = Guid.Parse("33333333-cccc-dddd-eeee-333333333333"), // Charlie
                        Title = "Underground Parking Lot",
                        Description = "Underground parking with gate access and cameras.",
                        Address = "789 Oak Ave, Midtown",
                        Latitude = 37.7749f,
                        Longitude = -122.4194f,
                        ImageUrl = null,
                        PricePerHour = 18.50m,
                        IsActive = false,
                        DateCreated = new DateTime(2024, 03, 10)
                    },
                    new Garage
                    {
                        Id = Guid.Parse("aaaa1111-0000-0000-0000-000000000004"),
                        OwnerId = Guid.Parse("44444444-dddd-eeee-ffff-444444444444"), // Diana
                        Title = "Garage Near Train Station",
                        Description = "Perfect for commuters — safe parking right next to the train.",
                        Address = "101 Central Blvd, Station City",
                        Latitude = 41.8781f,
                        Longitude = -87.6298f,
                        ImageUrl = "https://example.com/garage4.jpg",
                        PricePerHour = 12.00m,
                        IsActive = true,
                        DateCreated = new DateTime(2024, 04, 25)
                    }
                );
        }
    }


}
