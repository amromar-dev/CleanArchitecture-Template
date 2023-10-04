namespace CleanArchitectureTemplate.Application.Common.IdentityServices;

public interface ITokenService
{
	Task<(string AccessToken, double ExpiresIn, string RefreshToken)> GenerateToken(int userId, string username);
	Task<(int UserId, string AccessToken, double ExpiresIn, string RefreshToken)> GenerateToken(string refreshToken);
}
