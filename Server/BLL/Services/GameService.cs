using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GameService(IUnitOfWork unoOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unoOfWork;
        }

        public async Task<IEnumerable<GameModel>> GetAllAsync(string? genreSort, string? nameSort)
        {
            var games = await _unitOfWork
                              .GameRepository
                              .GetAllWithDetailsAsync(genreSort ?? "", nameSort ?? "");
            if (games == null || !games.Any())
                throw new GameStoreException("No games in store");
            return _mapper.Map<IEnumerable<Game>, IEnumerable<GameModel>>(games);
        }

        public async Task<GameModel> GetByIdAsync(Guid id)
        {
            var game = await _unitOfWork.GameRepository.GetByIdAsync(id);
            if (game == null)
                throw new GameStoreException("No such game in db");
            return _mapper.Map<GameModel>(game);
        }

        public async Task<GameModel> AddAsync(GameModel model)
        {
            var gameGuid = Guid.NewGuid();
            model.Id = gameGuid;
            await _unitOfWork.GameRepository.AddAsync(_mapper.Map<Game>(model));
            if (model.GenreIds != null && model.GenreIds.Any())
            {
                var gameGenre = model.GenreIds?
                    .Select(x => new GameGenre { GameId = gameGuid, GenreId = x });
                await _unitOfWork.GameGenreRepository.AddRangeAsync(gameGenre);
            }
            await _unitOfWork.SaveAsync();
            return model;
        }

        public async Task UpdateAsync(GameModel model)
        {
            var updatedGame = GetUpdatedGame(model).Result;
            _unitOfWork.GameRepository.Update(updatedGame);
            await UpdateGameGenres(model);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(GameModel model)
        {
            _unitOfWork.GameRepository.Delete(_mapper.Map<Game>(model));
            await _unitOfWork.SaveAsync();
        }

        private async Task<Game> GetUpdatedGame(GameModel model)
        {
            var game = await _unitOfWork.GameRepository.GetByIdWithNoTrack(model.Id ?? Guid.Empty);
            if (game == null)
                throw new GameStoreException($"Not found such game, probably id - {model.Id} is invalid");
            game.Title = model.Title;
            game.Description = model.Description;
            game.Price = model.Price;
            return game;
        }

        private async Task UpdateGameGenres(GameModel model)
        {
            if(model.GenreIds == null)
                return;
            var gameGenre = model.GenreIds?
                .Select(x => new GameGenre { GameId = model.Id ?? Guid.Empty, GenreId = x });
            var existedGenresOfGame = (await _unitOfWork.GameGenreRepository.GetAllAsync())
                .Where(g => g.GameId == model.Id);
            _unitOfWork.GameGenreRepository.DeleteRange(existedGenresOfGame.Except(gameGenre));
            await _unitOfWork.GameGenreRepository.AddRangeAsync(gameGenre.Except(existedGenresOfGame));
        }
    }
}
