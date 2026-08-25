namespace Examination.Infrastructure.SeedWork;

public class ExamSettings
{
    public string IdentityUrl { get; set; } = string.Empty;

    public DatabaseSettings DatabaseSettings { get; set; } = new();
}

public class DatabaseSettings
{
    public string Server { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
