using CleanArchitectureTemplate.Infrastructure.Persistence.Repositories.Base;
using CleanArchitectureTemplate.Infrastructure.Persistence.Models.RefreshTokens.Interfaces;
using CleanArchitectureTemplate.Infrastructure.Persistence.Models.RefreshTokens;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Repositories
{
	public class RefreshTokenRepository : BaseRepository<RefreshToken, string>, IRefreshTokenRepository
	{
		public RefreshTokenRepository(EFDbContext context) : base(context)
		{
		}
	}
}
