using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class MMDbContextFactory : IDesignTimeDbContextFactory<MMDbContext>
    {
        public MMDbContext CreateDbContext(string[] args)
        {
            // Build configuration to read the connection string from appsettings.json
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).FullName)  // One level up to the solution folder
                 .AddJsonFile("API/appsettings.json", optional: false, reloadOnChange: true) // Accessing appsettings.json from the 'API' folder
                 .Build();

            // Configure DbContextOptions with the connection string
            var optionsBuilder = new DbContextOptionsBuilder<MMDbContext>();
            optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 30)));

            return new MMDbContext(optionsBuilder.Options);
        }
    }
}

