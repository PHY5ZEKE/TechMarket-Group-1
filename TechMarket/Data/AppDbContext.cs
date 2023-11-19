using TechMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TechMarket.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        
            
            public DbSet<Product> Products { get; set; }
        public DbSet<Purchases> PurchasedProducts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
              

            



            }
        }
    }

