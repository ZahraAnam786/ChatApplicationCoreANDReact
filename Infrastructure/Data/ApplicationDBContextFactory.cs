using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDBContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
    {
        //For Migration add in this folder bcz not have connection in here so that add it
        //Entity Framework Core needs a way to create an instance of your DbContext. At runtime, this is easy because ASP.NET Core’s dependency injection system does that.
        public ApplicationDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDBContext>();
            optionsBuilder.UseSqlServer(@"Server=.;Database=ChatAppDB;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=true");
            return new ApplicationDBContext(optionsBuilder.Options);
        }
    }
}
