using DivineKids.Application.Common;
using DivineKids.Application.Contracts;
using DivineKids.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DivineKids.Infrastructure.Identity;

internal sealed class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    RoleManager<ApplicationRole> roleManager,            // Added
    IOptions<JwtSettings> jwtOptions,
    AppDbContext dbContext,
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    private readonly JwtSettings _jwt = jwtOptions.Value;

    public async Task<Result<AuthTokens>> RegisterAsync(string name, string email, string password, string phoneNumber)
    {
        var existing = await userManager.FindByEmailAsync(email);
        if (existing is not null)
            return Result<AuthTokens>.Failure("Email already registered.");

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            Name = name,
            EmailConfirmed = true,
            PhoneNumber = phoneNumber,
        };

        var createResult = await userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            var error = string.Join("; ", createResult.Errors.Select(e => e.Description));
            return Result<AuthTokens>.Failure(error);
        }

        // Assign default roles automatically
        var defaultRoleNames = await roleManager.Roles
            .Where(r => r.IsDefaultForNewUsers)
            .Select(r => r.Name!)
            .ToListAsync();

        if (defaultRoleNames.Count == 0 && await roleManager.RoleExistsAsync(RoleNames.User))
        {
            defaultRoleNames.Add(RoleNames.User);
        }

        if (defaultRoleNames.Count > 0)
        {
            var addRolesResult = await userManager.AddToRolesAsync(user, defaultRoleNames);
            if (!addRolesResult.Succeeded)
            {
                var roleErrors = string.Join("; ", addRolesResult.Errors.Select(e => e.Description));
                return Result<AuthTokens>.Failure($"User created but failed assigning roles: {roleErrors}");
            }
        }

        var tokens = await IssueTokensAsync(user);
        return Result<AuthTokens>.Success(tokens);
    }

    public async Task<Result<AuthTokens>> LoginAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result<AuthTokens>.Failure("Invalid credentials.");

        var signIn = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
        if (!signIn.Succeeded)
            return Result<AuthTokens>.Failure("Invalid credentials.");

        var tokens = await IssueTokensAsync(user);
        return Result<AuthTokens>.Success(tokens);
    }

    public async Task<Result<AuthTokens>> RefreshAsync(string refreshToken)
    {
        var stored = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == refreshToken);

        if (stored is null)
            return Result<AuthTokens>.Failure("Invalid refresh token.");

        if (!stored.IsActive)
            return Result<AuthTokens>.Failure("Refresh token inactive or expired.");

        var user = await userManager.FindByIdAsync(stored.UserId.ToString());
        if (user is null)
            return Result<AuthTokens>.Failure("User no longer exists.");

        // rotate
        stored.RevokedAtUtc = DateTime.UtcNow;
        stored.ReplacedByToken = "rotated";
        var newTokens = await IssueTokensAsync(user);

        await dbContext.SaveChangesAsync();
        return Result<AuthTokens>.Success(newTokens);
    }

    public async Task<Result> RevokeAsync(string refreshToken)
    {
        var stored = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == refreshToken);

        if (stored is null)
            return Result.Failure("Token not found.");

        if (!stored.IsActive)
            return Result.Failure("Token already inactive.");

        stored.RevokedAtUtc = DateTime.UtcNow;
        stored.RevokedByIp = GetIp();
        await dbContext.SaveChangesAsync();
        return Result.Success();
    }

    private async Task<AuthTokens> IssueTokensAsync(ApplicationUser user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var accessToken = GenerateJwt(user, roles.ToList().AsReadOnly(), out var expiresAt);
        var refreshToken = CreateRefreshToken(user);
        await dbContext.RefreshTokens.AddAsync(refreshToken);
        await dbContext.SaveChangesAsync();
        return new AuthTokens(user.Name, accessToken, refreshToken.Token, expiresAt);
    }

    private string GenerateJwt(ApplicationUser user, IReadOnlyCollection<string> roles, out DateTime expiresAtUtc)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Name,user.Name),
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add role claims
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        expiresAtUtc = DateTime.UtcNow.AddMinutes(_jwt.ExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: expiresAtUtc,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private RefreshToken CreateRefreshToken(ApplicationUser user)
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        var token = Convert.ToBase64String(tokenBytes)
            .Replace("+", "-", StringComparison.Ordinal)
            .Replace("/", "_", StringComparison.Ordinal)
            .TrimEnd('=');

        return new RefreshToken
        {
            UserId = user.Id,
            Token = token,
            CreatedAtUtc = DateTime.UtcNow,
            CreatedByIp = GetIp(),
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_jwt.RefreshTokenExpirationDays)
        };
    }

    private string? GetIp() => httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
}