using BLL.Models;

namespace BLL.Interfaces
{
    public interface IGenreService
    {
        public Task<IEnumerable<GenreModel>> GetAllAsync();
        public Task<IEnumerable<GenreModel>> GetAllByGameId(Guid gameId);
        public Task<GenreModel> GetGenreById(Guid id);
    }
}
