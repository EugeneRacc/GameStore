using DAL.Entities;

namespace DAL.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetByGameIdAsync(Guid id);
    }
}
