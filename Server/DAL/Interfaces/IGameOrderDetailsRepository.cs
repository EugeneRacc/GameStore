using DAL.Entities;

namespace DAL.Interfaces;

public interface IGameOrderDetails
{
    Task<IEnumerable<GameOrderDetails>> GetAllAsync();
    Task AddAsync(GameOrderDetails entity);
    Task AddRangeAsync(IEnumerable<GameOrderDetails> entities);
    void Delete(GameOrderDetails entity);
}