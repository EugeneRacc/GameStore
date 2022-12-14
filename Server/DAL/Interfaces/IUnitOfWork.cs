namespace DAL.Interfaces
{
    public interface IUnitOfWork
    {
        ICommentRepository  CommentRepository { get; }
        IGameRepository GameRepository { get; }
        IGameImageRepository GameImage { get; }
        IGenreRepository GenreRepository { get; }
        IGameGenreRepository GameGenreRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IOrderDetailsRepository OrderDetailsRepository { get; }
        IGameOrderDetails GameOrderDetails { get; }
        Task SaveAsync();
    }
}
