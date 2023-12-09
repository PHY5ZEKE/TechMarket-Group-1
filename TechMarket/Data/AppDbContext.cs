using TechMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TechMarket.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchases> PurchasedProducts { get; set; }
        public DbSet<ToShipProduct> ToShipProducts { get; set; }
        public DbSet<ToReceiveProduct> ToReceiveProducts { get; set; }

        public DbSet<Review> Reviews { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
           
            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(null, "Test123!"); // Change "Test123!" to the desired password


            modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = "ea68465f-1dba-4e0b-96db-1aee74bc4743", // You might want to generate a unique ID
                UserName = "administrator",
                NormalizedUserName = "ADMINISTRATOR", // Uppercase version of UserName
                FirstName = "Techmarket",
                LastName = "Administrator",
                Email = "admin123@gmail.com",
                NormalizedEmail = "ADMIN123@GMAIL.COM", // Uppercase version of Email
                Address = "TechMarket Company",
                Birthday = new DateTime(2001, 7, 9),
                Phone = "09762909844", // Add the contact number
                IdPictureUrl = "profile/id/admin id.png",
               ProfilePictureUrl = "profile/pfp/admin.jpg",
                PasswordHash = hashedPassword
            });



        }
        }
    }

