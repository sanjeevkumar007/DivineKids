namespace DivineKids.Infrastructure.Email;
public sealed class EmailSettings
{
    public const string SectionName = "Email";
    public string FromName { get; init; } = "";
    public string FromAddress { get; init; } = "";
    public string Host { get; init; } = "";
    public int Port { get; init; }
    public bool UseSsl { get; init; }
    public string User { get; init; } = "";
    public string Password { get; init; } = "";
}