using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class GameOrderDetailsRepository : IGameOrderDetails
{
    private readonly GameStoreDbContext _dbContext;

    public GameOrderDetailsRepository(GameStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<GameOrderDetails>> GetAllAsync()
    {
        return await _dbContext.GameOrderDetails.ToListAsync();
    }

    public async Task AddAsync(GameOrderDetails entity)
    {
        await _dbContext.GameOrderDetails.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<GameOrderDetails> entities)
    {
        await _dbContext.GameOrderDetails.AddRangeAsync(entities);
    }

    public void Delete(GameOrderDetails entity)
    {
        _dbContext.GameOrderDetails.Remove(entity);
    }
}