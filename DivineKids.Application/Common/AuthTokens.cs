namespace DivineKids.Application.Common;
public sealed record AuthTokens(
    string name,
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAtUtc);