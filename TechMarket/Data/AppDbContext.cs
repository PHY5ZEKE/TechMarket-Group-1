using TechMarket.Models;
using Microsoft.EntityFrameworkCore;

namespace TechMarket.Data
{
    public class AppDbContext : DbContext
    {
        
            public DbSet<Account> Accounts { get; set; }
           
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
                        Birthdate = DateTime.Parse("09/07/2001"),
                        ContactNo = "09762909844"

                    }


                  );


            }
        }
    }

