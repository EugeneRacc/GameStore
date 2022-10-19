using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using DAL.Interfaces;

namespace BLL.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public GenreService(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public  async Task<IEnumerable<GenreModel>> GetAllAsync()
        {
            var genres = await _db.GenreRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GenreModel>>(genres);
        }

        public async Task<IEnumerable<GenreModel>> GetAllByGameIdAsync(Guid gameId)
        {
            var genres = await _db.GenreRepository.GetGenresByGameIdAsync(gameId);
            return _mapper.Map<IEnumerable<GenreModel>>(genres);
        }

        public async Task<GenreModel> GetGenreByIdAsync(Guid id)
        {
            var genre = await _db.GenreRepository.GetByIdAsync(id);
            return _mapper.Map<GenreModel>(genre);
        }
    }
}
