using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(GameStoreDbContext db) : base(db) {}
        public async Task<RefreshToken> GetRefreshTokenByToken(string token)
        {
            return await db.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        }
    }
}
