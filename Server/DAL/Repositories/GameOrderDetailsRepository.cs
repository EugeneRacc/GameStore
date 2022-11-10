using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories;

public class GameOrderDetailsRepository : IGameOrderDetails
{
    private readonly GameStoreDbContext _dbContext;

    public GameOrderDetailsRepository(GameStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<IEnumerable<GameGenre>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(GameGenre entity)
    {
        throw new NotImplementedException();
    }

    public Task AddRangeAsync(IEnumerable<GameGenre> entities)
    {
        throw new NotImplementedException();
    }

    public void Delete(GameGenre entity)
    {
        throw new NotImplementedException();
    }
}