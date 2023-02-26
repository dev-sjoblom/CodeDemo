namespace CommunicationService.Test.Fundamental.Database;

public class AutoTestDbOptions
{
    public const string Section = "AutoTestDbOptions";
    public required string Host { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string DbPrefix { get; set; }

    public string? GetConnectionString() => $"host={Host};database={DbPrefix};username={Username};password={Password}";
}