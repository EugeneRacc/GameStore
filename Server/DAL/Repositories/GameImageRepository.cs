using DAL.Data;
using DAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class GameImageRepository : IGameImageRepository
    {
        private readonly GameStoreDbContext _db;

        public GameImageRepository(GameStoreDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<IEnumerable<GameImage>> GetAllAsync()
        {
            return await _db.GameImages.ToListAsync();
        }

        public async Task<GameImage> GetByIdAsync(Guid id)
        {
            return await _db.GameImages.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(GameImage entity)
        {
            await _db.GameImages.AddAsync(entity);
        }

        public void Delete(GameImage entity)
        {
            _db.GameImages.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var model = await GetByIdAsync(id);
            _db.GameImages.Remove(model);
        }

        public void Update(GameImage entity)
        {
            EntityEntry entityEntry = _db.Entry<GameImage>(entity);
            entityEntry.State = EntityState.Modified;
        }

        public async Task<GameImage> GetByIdWithNoTrack(Guid id)
        {
            return await _db.GameImages.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
