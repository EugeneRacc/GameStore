using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly GameStoreDbContext db;

        public Repository(GameStoreDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await db.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await db.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(TEntity entity)
        {
            await db.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            db.Set<TEntity>().Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var model = await GetByIdAsync(id);
            db.Set<TEntity>().Remove(model);
        }

        public void Update(TEntity entity)
        {
            EntityEntry entityEntry = db.Entry<TEntity>(entity);
            entityEntry.State = EntityState.Modified;
        }

    }
}
