using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class CommentRepository : Repository<Comment> ,ICommentRepository
    {
        public CommentRepository(GameStoreDbContext db) : base(db) { }
        public async Task<IEnumerable<Comment>> GetByGameIdAsync(Guid id)
        {
            var comments = await db.Comments.Where(c => c.GameId == id)
                                   .ToListAsync();
            return comments;
        }
    }
}
