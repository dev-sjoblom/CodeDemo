namespace CommunicationService.Test.Fundamental.Database;

public class AutoTestDbOptions
{
    public const string Section = "AutoTestDbOptions";
    public required string Host { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string DbPrefix { get; set; }

    public string? GetConnectionString(string testInstance) => $"host={Host};database={DbPrefix}{testInstance};username={Username};password={Password}";
}