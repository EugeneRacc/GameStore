using BLL.Models;

namespace BLL.Interfaces
{
    public interface ICommentService : ICrud<CommentModel>
    {
        Task<IEnumerable<CommentModel>> GetGameComments(Guid gameId);
    }
}
