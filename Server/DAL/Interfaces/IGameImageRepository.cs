using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IGameImageRepository : IRepository<GameImage>
    {
        public Task<IEnumerable<GameImage>> GetImagesByGameId(Guid gameId);
    }
}
