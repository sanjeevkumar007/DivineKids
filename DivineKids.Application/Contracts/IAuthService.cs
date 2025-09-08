using DivineKids.Application.Common;

namespace DivineKids.Application.Contracts;

public interface IAuthService
{
    Task<Result<AuthTokens>> RegisterAsync(string name, string email, string password, string phoneNumber);
    Task<Result<AuthTokens>> LoginAsync(string email, string password);
    Task<Result<AuthTokens>> RefreshAsync(string refreshToken);
    Task<Result> RevokeAsync(string refreshToken);

}