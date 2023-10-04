using CleanArchitectureTemplate.SharedKernel.Types;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Models.RefreshTokens
{
    public class RefreshToken : BaseAuditEntity<string>
    {
        private RefreshToken()
        {

        }

        public RefreshToken(int userId, string username)
        {
            Id = Guid.NewGuid().ToString().Replace("-", "");
            UserId = userId;
            Username = username;
            ExpireAt = DateTime.UtcNow.AddDays(30);
        }

        public int UserId { get; private set; }

        public string Username { get; private set; }

        public DateTime ExpireAt { get; private set; }
    }
}
