using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class GameRepository : Repository<Game>, IGameRepository
    {
        public GameRepository(GameStoreDbContext dbContext) : base(dbContext) { }

        public async Task<Game> GetByIdWithDetailsAsync(Guid id)
        {
            return await db.Games
                            .Include(gi => gi.GameImages)
                            .Include(gg => gg.GameGenres)
                            .ThenInclude(g => g.Genre)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Game> GetByIdWithDetailsWithNoTrack(Guid id)
        {
            return await db.Games.AsNoTracking()
                            .Include(gi => gi.GameImages)
                            .Include(gg => gg.GameGenres)
                            .ThenInclude(g => g.Genre)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Game>> GetAllWithDetailsAsync(string genreSort, string nameSort)
        {
            return await db.Games
                             .Include(gi => gi.GameImages)
                             .Include(gg => gg.GameGenres)
                             .ThenInclude(g => g.Genre)
                             .Where(game => game.Title.Contains(nameSort))
                             .Where(g => g.GameGenres.Any(x => x.Genre.Title.Contains(genreSort)) 
                             || g.GameGenres.Count == 0)
                             .ToListAsync();
        }
    }
}
