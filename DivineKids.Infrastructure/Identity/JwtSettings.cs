﻿namespace DivineKids.Infrastructure.Identity;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Key { get; init; } = string.Empty;
    public int ExpirationMinutes { get; init; } = 60;
    public int RefreshTokenExpirationDays { get; init; } = 7;
}