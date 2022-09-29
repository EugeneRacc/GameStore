using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly GameStoreDbContext _db;

        public CommentRepository(GameStoreDbContext db)
        {
            _db = db;
        }


        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _db.Comments.ToListAsync();
        }

        public async Task<Comment> GetByIdAsync(Guid id)
        {
            return await _db.Comments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async  Task AddAsync(Comment entity)
        {
            await _db.Comments.AddAsync(entity);
        }

        public void Delete(Comment entity)
        {
            _db.Comments.Remove(entity);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var model = await GetByIdAsync(id);
            _db.Comments.Remove(model);
        }

        public void Update(Comment entity)
        {
            EntityEntry entityEntry = _db.Entry<Comment>(entity);
            entityEntry.State = EntityState.Modified;
        }

        public async Task<Comment> GetByIdWithNoTrack(Guid id)
        {
            return await _db.Comments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
