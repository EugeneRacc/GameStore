using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IGameGenreRepository
    {
        Task<IEnumerable<GameGenre>> GetAllAsync();
        Task AddAsync(GameGenre entity);
        Task AddRangeAsync(IEnumerable<GameGenre> entities);
        void Delete(GameGenre entity);
        void DeleteRange(IEnumerable<GameGenre> entity);
        void Update(GameGenre entity);
    }
}
