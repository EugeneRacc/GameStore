using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IGenreRepository : IRepository<Genre>
    {
        public Task<IEnumerable<Genre>> GetGenresByGameIdAsync(Guid gameId);
    }
}
