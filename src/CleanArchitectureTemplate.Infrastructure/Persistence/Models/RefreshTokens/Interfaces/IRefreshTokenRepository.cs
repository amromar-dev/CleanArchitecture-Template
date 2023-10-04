using CleanArchitectureTemplate.SharedKernel.Interfaces;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Models.RefreshTokens.Interfaces
{
    public interface IRefreshTokenRepository : IBaseRepository<RefreshToken, string>
    {
    }
}
