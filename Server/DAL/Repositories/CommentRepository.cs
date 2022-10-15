using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class CommentRepository : Repository<Comment> ,ICommentRepository
    {
        public CommentRepository(GameStoreDbContext db) : base(db) { }
    }
}
