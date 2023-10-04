using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CleanArchitectureTemplate.Application.Common.IdentityServices;
using CleanArchitectureTemplate.Infrastructure.Persistence.Models.RefreshTokens;
using CleanArchitectureTemplate.Infrastructure.Persistence.Models.RefreshTokens.Interfaces;
using CleanArchitectureTemplate.SharedKernel.Types;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitectureTemplate.Infrastructure.Services.IdentityServices
{
	public class BearerTokenService : ITokenService
	{
		private readonly IConfiguration configuration;
		private readonly IRefreshTokenRepository refreshTokenRepository;

		public BearerTokenService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
		{
			this.configuration = configuration;
			this.refreshTokenRepository = refreshTokenRepository;
		}

		public async Task<(string AccessToken, double ExpiresIn, string RefreshToken)> GenerateToken(int userId, string username)
		{
			var (accessToken, expiresIn) = GenerateAccessToken(userId, username);
			var refreshToken = await CreateRefreshToken(userId, username);

			return (accessToken, expiresIn, refreshToken);
		}

		public async Task<(int UserId, string AccessToken, double ExpiresIn, string RefreshToken)> GenerateToken(string refreshTokenId)
		{
			var user = await ValidateRefreshToken(refreshTokenId);
			var response = await GenerateToken(user.UserId, user.Username);

			return (user.UserId, response.AccessToken, response.ExpiresIn, response.RefreshToken);
		}


		#region Private Methods

		private (string AccessToken, double ExpiresIn) GenerateAccessToken(int userId, string username)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);
			var utcNow = DateTime.UtcNow;

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, username),
					new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
				}),
				Expires = utcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
				Issuer = configuration["Jwt:Issuer"],
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			var accessToken = tokenHandler.WriteToken(token);
			var expiresIn = token.ValidTo.Subtract(token.ValidFrom).TotalSeconds;

			return (accessToken, expiresIn);
		}

		private async Task<string> CreateRefreshToken(int userId, string username)
		{
			var refreshToken = new RefreshToken(userId, username);
			refreshTokenRepository.Add(refreshToken);

			await refreshTokenRepository.CommitAsync();

			return refreshToken.Id.ToString();
		}

		public async Task<(int UserId, string Username)> ValidateRefreshToken(string refreshTokenId)
		{
			var refreshToken = await refreshTokenRepository.FindAsync(refreshTokenId);
			if (refreshToken == null)
				throw new BusinessException(System.Net.HttpStatusCode.Unauthorized, "Not Valid Refresh Token");

			if (refreshToken.ExpireAt < DateTime.UtcNow)
				throw new BusinessException(System.Net.HttpStatusCode.Unauthorized, "Not Valid Refresh Token");

			refreshTokenRepository.Remove(refreshToken);
			return (refreshToken.UserId, refreshToken.Username);
		}
		#endregion
	}
}
