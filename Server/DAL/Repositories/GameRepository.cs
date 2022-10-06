using System.Security.Cryptography.X509Certificates;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DAL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly GameStoreDbContext _db;

        public GameRepository(GameStoreDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _db.Games.ToListAsync();
        }

        public async Task<Game> GetByIdAsync(Guid id)
        {
            return await _db.Games
                            .Include(gg => gg.GameGenres)
                            .ThenInclude(g => g.Genre)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Game entity)
        {
            await _db.Games.AddAsync(entity);
        }

        public void Delete(Game entity)
        {
            _db.Games.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var model = await GetByIdAsync(id);
            _db.Games.Remove(model);
        }

        public void Update(Game entity)
        {
            EntityEntry entityEntry = _db.Entry<Game>(entity);
            entityEntry.State = EntityState.Modified;
        }

        public async Task<Game> GetByIdWithNoTrack(Guid id)
        {
            return await _db.Games.AsNoTracking()
                            .Include(gg => gg.GameGenres)
                            .ThenInclude(g => g.Genre)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Game>> GetAllWithDetailsAsync(string genreSort, string nameSort)
        {
            return await _db.Games
                             .Include(gg => gg.GameGenres)
                             //.Where(game => game.Genre.Title.Contains(genreSort) 
                             //   || game.Genre.MainGenre.Title.Contains(genreSort)))
                             .ThenInclude(g => g.Genre)
                             .Where(game => game.Title.Contains(nameSort))
                             .Where(g => g.GameGenres.Any(x => x.Genre.Title.Contains(genreSort)))
                             .ToListAsync();
        }
    }
}
