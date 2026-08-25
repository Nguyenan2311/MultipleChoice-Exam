using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.Services;
using Identity.API;
using Identity.API.Database;
using Identity.API.Extentions;
using Identity.API.Model;
using Identity.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets(typeof(Program).Assembly, optional: true);
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        }));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<AppSetting>(builder.Configuration);

builder.Services.AddIdentityServer(options =>
{
    options.IssuerUri = "https://localhost:5001";
    options.Authentication.CookieLifetime = TimeSpan.FromHours(2);
})
.AddDeveloperSigningCredential()
.AddAspNetIdentity<ApplicationUser>()
.AddConfigurationStore(options =>
{
    options.ConfigureDbContext = b => b.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        });
})
.AddOperationalStore(options =>
{
    options.ConfigureDbContext = b => b.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        });
});

builder.Services.AddTransient<IProfileService, ProfileService>();

var app = builder.Build();

app.MigrateDbContext<ApplicationDbContext>((context, services) =>
{
    var env = services.GetRequiredService<IWebHostEnvironment>();
    var logger = services.GetRequiredService<ILogger<ApplicationDbContextSeed>>();
    var settings = services.GetRequiredService<IOptions<AppSetting>>();

    var seed = new ApplicationDbContextSeed();
    seed.SeedAsync(context, env, logger, settings).GetAwaiter().GetResult();
});

app.MigrateDbContext<ConfigurationDbContext>((context, services) =>
{
    var configuration = services.GetRequiredService<IConfiguration>();
    var seed = new ConfigurationDbContextSeed();
    seed.SeedAsync(context, configuration).GetAwaiter().GetResult();
});

app.MigrateDbContext<PersistedGrantDbContext>(
    (context, services) =>
    {
    });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();
