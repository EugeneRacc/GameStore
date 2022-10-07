using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories
{
    public class GenreRepository : Repository<Genre>,IGenreRepository
    {
        public GenreRepository(GameStoreDbContext dbContext) : base(dbContext) { }
    }
}
