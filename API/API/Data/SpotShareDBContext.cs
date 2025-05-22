using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class SpotShareDBContext(DbContextOptions<SpotShareDBContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(

                    new User
                    {
                        Id = Guid.Parse("11111111-aaaa-bbbb-cccc-111111111111"),
                        Name = "Alice Johnson",
                        Password = "hashed_password_1", // Normally store hashed passwords
                        Email = "alice@example.com",
                        Phone = "555-1234",
                        DateCreated = new DateTime(2024, 01, 15),
                        RoleId = 1
                    },
                    new User
                    {
                        Id = Guid.Parse("22222222-bbbb-cccc-dddd-222222222222"),
                        Name = "Bob Smith",
                        Password = "hashed_password_2",
                        Email = "bob@example.com",
                        Phone = "555-5678",
                        DateCreated = new DateTime(2024, 02, 10),
                        RoleId = 2
                    },
                    new User
                    {
                        Id = Guid.Parse("33333333-cccc-dddd-eeee-333333333333"),
                        Name = "Charlie Evans",
                        Password = "hashed_password_3",
                        Email = "charlie@example.com",
                        Phone = null,
                        DateCreated = new DateTime(2024, 03, 05),
                        RoleId = 1
                    },
                    new User
                    {
                        Id = Guid.Parse("44444444-dddd-eeee-ffff-444444444444"),
                        Name = "Diana Carter",
                        Password = "hashed_password_4",
                        Email = "diana@example.com",
                        Phone = "555-8765",
                        DateCreated = new DateTime(2024, 04, 22),
                        RoleId = 3
                    }
                );

        }
    }


}
