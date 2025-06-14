using Domain.Entities;
using Domain.Entities.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDBContext : DbContext
    {

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserMessages> UserMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductMap());

            modelBuilder.ApplyConfiguration(new UserMap());

            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductMap).Assembly);

            //modelBuilder.Entity<Product>().HasData(
            //   new Product { Id = 1, ProductName = "Chai", UnitPrice = 1 },
            //   new Product { Id = 2, ProductName = "Chang", UnitPrice = 2 },
            //   new Product { Id = 3, ProductName = "Cappuccino", UnitPrice = 3 }
           //);
        }

    }
}
