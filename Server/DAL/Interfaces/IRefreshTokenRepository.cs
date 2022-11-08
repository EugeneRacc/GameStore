using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken> GetRefreshTokenByToken (string token);
    }
}
