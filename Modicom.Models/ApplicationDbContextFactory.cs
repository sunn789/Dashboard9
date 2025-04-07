
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Modicom.Models.Entities;
namespace Modicom.Models;


public class BloggingContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory(); // Or adjust as needed
        Console.WriteLine($"Base path: {basePath}"); // Debugging
         basePath = Path.Combine(
            Directory.GetCurrentDirectory(), 
            "../Modicom.Raz"  // Adjust based on actual structure
        );

              var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();
       // 3. Configure PostgreSQL
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = configuration.GetConnectionString("ApplicationDbContextConnection");
        optionsBuilder.UseNpgsql(connectionString); // Ensure this is PostgreSQL

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}