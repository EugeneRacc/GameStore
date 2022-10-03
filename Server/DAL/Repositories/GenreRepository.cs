using System.Reflection.Emit;
using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly GameStoreDbContext _db;

        public GenreRepository(GameStoreDbContext dbContext)
        {
            _db = dbContext;
        }


        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _db.Genres.ToListAsync();
        }

        public async Task<Genre> GetByIdAsync(Guid id)
        {
            return await _db.Genres.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Genre entity)
        {
            await _db.Genres.AddAsync(entity);
        }

        public void Delete(Genre entity)
        {
            _db.Genres.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var model = await GetByIdAsync(id);
            _db.Genres.Remove(model);
        }

        public void Update(Genre entity)
        {
            EntityEntry entityEntry = _db.Entry<Genre>(entity);
            entityEntry.State = EntityState.Modified;
        }

        public async Task<Genre> GetByIdWithNoTrack(Guid id)
        {
            return await _db.Genres.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
