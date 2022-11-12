using DAL.Entities;

namespace DAL.Interfaces;

public interface IGameOrderDetails
{
    Task<IEnumerable<GameGenre>> GetAllAsync();
    Task AddAsync(GameGenre entity);
    Task AddRangeAsync(IEnumerable<GameGenre> entities);
    void Delete(GameGenre entity);
}