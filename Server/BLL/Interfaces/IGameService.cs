using BLL.Models;

namespace BLL.Interfaces
{
    public interface IGameService : ICrud<GameModel>
    {
        Task<IEnumerable<GameModel>> GetAllAsync(string? genreSort, string? nameSort);
    }
}
