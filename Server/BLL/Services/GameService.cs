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

        public async Task<IEnumerable<GameModel>> GetAllAsync()
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();
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
            model.Id = Guid.NewGuid();
            await _unitOfWork.GameRepository.AddAsync(_mapper.Map<Game>(model));
            await _unitOfWork.SaveAsync();
            return model;
        }

        public async Task UpdateAsync(GameModel model)
        {
            var game = await _unitOfWork.GameRepository.GetByIdWithNoTrack(model.Id ?? Guid.Empty);
            if (game == null)
                throw new GameStoreException($"Not found such game, probably id - {model.Id} is invalid");
            game.Title = model.Title;
            game.Description = model.Description;
            game.Price = model.Price;
            _unitOfWork.GameRepository.Update(_mapper.Map<Game>(game));
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(GameModel model)
        {
            _unitOfWork.GameRepository.Delete(_mapper.Map<Game>(model));
            await _unitOfWork.SaveAsync();
        }
    }
}
