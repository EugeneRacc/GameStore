using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DAL.Repositories
{
    public class GameGenreRepository : IGameGenreRepository
    {
        private readonly GameStoreDbContext _db;

        public GameGenreRepository(GameStoreDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<IEnumerable<GameGenre>> GetAllAsync()
        {
            return await _db.GameGenre.ToListAsync();
        }

        public async Task AddAsync(GameGenre entity)
        {
            await _db.GameGenre.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<GameGenre> gameGenres)
        {
            await _db.GameGenre.AddRangeAsync(gameGenres);
        }

        public void DeleteRange(IEnumerable<GameGenre> gameGenres)
        {
            _db.GameGenre.RemoveRange(gameGenres);
        }

        public void Delete(GameGenre entity)
        {
            _db.GameGenre.Remove(entity);
        }

        public void Update(GameGenre entity)
        {
            EntityEntry entityEntry = _db.Entry<GameGenre>(entity);
            entityEntry.State = EntityState.Modified;
        }
    }
}
