using Identity.API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.API.Database;

public class ApplicationDbContextSeed
{
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();
    public async Task SeedAsync(ApplicationDbContext context, IWebHostEnvironment env, ILogger<ApplicationDbContextSeed> logger, IOptions<AppSetting> settings, int? retry = 0)
    {
        if (retry != null)
        {
            int retryForAvailability = retry.Value;
            try
            {
                if (!context.Users.Any())
                {
                    context.Users.AddRange(GetDefaultUsers());
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    logger.LogError(ex, "Exception occurred while seeding the database. Retrying... Attempt {RetryAttempt}", retryForAvailability);
                    await SeedAsync(context, env, logger, settings, retryForAvailability);
                }
                else
                {
                    logger.LogError(ex, "Exception occurred while seeding the database. Maximum retry attempts reached.");
                    throw; // Rethrow the exception after maximum retries
                }
            }
        }
        else
        {
            throw new ArgumentNullException(nameof(retry), "Retry parameter cannot be null.");
        }
    }
    private IEnumerable<ApplicationUser> GetDefaultUsers()
    {
        var user = new ApplicationUser()
        {
            Email = "admin@demo.com",
            Id = Guid.NewGuid().ToString(),
            LastName = "Account",
            FirstName = "Demo",
            PhoneNumber = "1234567890",
            UserName = "admin@demo.com",
            NormalizedEmail = "ADMIN@DEMO.COM",
            NormalizedUserName = "ADMIN@DEMO.COM",
            SecurityStamp = Guid.NewGuid().ToString("D"),


        };
        user.PasswordHash = _passwordHasher.HashPassword(user, "Admin@123");
        return new List<ApplicationUser>() { user };
    }
}
