using DAL.Interfaces;
using DAL.Repositories;

namespace DAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GameStoreDbContext _dbContext;
        private ICommentRepository _commentRepository; 
        private IGameRepository _gameRepository;
        private IGameImageRepository _gameImageRepository;
        private IGenreRepository _genreRepository;
        private IGameGenreRepository _gameGenreRepository;
        public UnitOfWork(GameStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ICommentRepository CommentRepository
        {
            get
            {
                if (_commentRepository == null)
                    _commentRepository = new CommentRepository(_dbContext);
                return _commentRepository;
            }
        }
        public IGameRepository GameRepository
        {
            get
            {
                if (_gameRepository == null)
                    _gameRepository = new GameRepository(_dbContext);
                return _gameRepository;
            }
        }
        public IGameImageRepository GameImage
        {
            get
            {
                if (_gameImageRepository == null)
                    _gameImageRepository = new GameImageRepository(_dbContext);
                return _gameImageRepository;
            }
        }
        public IGenreRepository GenreRepository
        {
            get
            {
                if (_genreRepository == null)
                    _genreRepository = new GenreRepository(_dbContext);
                return _genreRepository;
            }
        }

        public IGameGenreRepository GameGenreRepository
        {
            get
            {
                if (_gameGenreRepository == null)
                    _gameGenreRepository = new GameGenreRepository(_dbContext);
                return _gameGenreRepository;
            }
        }
        
        public async Task SaveAsync()
        {

            await _dbContext.SaveChangesAsync();
        }
       
    }
}
