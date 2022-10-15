using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class GenreRepository : Repository<Genre>,IGenreRepository
    {
        public GenreRepository(GameStoreDbContext dbContext) : base(dbContext) { }
        public async Task<IEnumerable<Genre>> GetGenresByGameIdAsync(Guid gameId)
        {
            return await db.Genres
                     .Include(gg => gg.GameGenres)
                     .Where(g => g.GameGenres.Any(x => x.GameId == gameId))
                     .ToListAsync();
        }
    }
}
