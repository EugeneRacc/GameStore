using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IGameRepository : IRepository<Game>
    {
        Task<IEnumerable<Game>> GetAllWithDetailsAsync(string genreSort, string nameSort);
        Task<Game> GetByIdWithDetailsAsync(Guid id);
        Task<Game> GetByIdWithDetailsWithNoTrack(Guid id);
    }
}
