using TechMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TechMarket.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        
            public DbSet<Account> Accounts { get; set; }
            public DbSet<Product> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.Entity<Account>().HasData(

                    new Account()
                    {
                        AcctId = 1,
                        FirstName = "Ezequiel",
                        LastName = "Gonzalez",
                        Email = "ezequiel.gonzalez.cics@ust.edu.ph",
                        Username = "phyzeke",
                        Password ="admin123",
                        Address = "Lubao, Pampanga",
                        Birthday = DateTime.Parse("09/07/2001"),
                        Phone = "09762909844"
                    }


                  );

            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    ProdId = 1,
                    AcctId = 1,
                    Username = "phyzeke",
                    ProdImage = "Wala pa",
                    ProdName = "Iphone 14",
                    ProdDesc ="Slightly Used",
                    ProdTags = ProdTags.Smartphones,
                    ProdQuantity = 1,
                    ProdPrice = "50,000"
                }
                );



            }
        }
    }

