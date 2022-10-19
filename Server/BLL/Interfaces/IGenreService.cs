using BLL.Models;

namespace BLL.Interfaces
{
    public interface IGenreService
    {
        public Task<IEnumerable<GenreModel>> GetAllAsync();
        public Task<IEnumerable<GenreModel>> GetAllByGameIdAsync(Guid gameId);
        public Task<GenreModel> GetGenreByIdAsync(Guid id);
    }
}
