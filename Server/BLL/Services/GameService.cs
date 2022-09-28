using AutoMapper;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Models;
using DAL.Data;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IMapper _mapper;
        private readonly GameStoreDbContext _db;

        public GameService(IMapper mapper, GameStoreDbContext dbContext)
        {
            _db = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GameModel>> GetAllAsync()
        {
            var games = await _db.Games.ToListAsync();
            if (games == null || games.Count == 0)
                throw new GameStoreException("No games in store");
            var mappedGames = _mapper.Map<IEnumerable<Game>, IEnumerable<GameModel>>(games);
            return mappedGames;
        }

        public async Task<GameModel> GetByIdAsync(Guid id)
        {
            var game = await _db.Games.FirstOrDefaultAsync(x => x.Id == id);
            if (game == null)
                throw new GameStoreException("No such game in db");
            return _mapper.Map<GameModel>(game);
        }

        public async Task<GameModel> AddAsync(GameModel model)
        {
            model.Id = Guid.NewGuid();
            await _db.Games.AddAsync(_mapper.Map<GameModel, Game>(model));
            await _db.SaveChangesAsync();
            return model;
        }

        public async Task UpdateAsync(GameModel model)
        {
            var game = await _db.Games.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Id);
            game.Title = model.Title;
            game.Description = model.Description;
            game.Price = model.Price;
            var mappedGame = _mapper.Map<Game>(game);

            EntityEntry entityEntry = _db.Entry<Game>(mappedGame);
            entityEntry.State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public void Delete(GameModel model)
        {
            _db.Games.Remove(_mapper.Map<Game>(model));
            _db.SaveChanges();
        }
    }
}
