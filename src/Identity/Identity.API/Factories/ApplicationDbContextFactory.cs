using System.Reflection;
using Identity.API.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.API.Factories;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
             .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
             .AddJsonFile("appsettings.json").AddEnvironmentVariables()
             .AddUserSecrets(typeof(Program).Assembly, optional: true)
             .Build();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"), sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
        });
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
