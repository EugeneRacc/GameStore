using BLL.Models;

namespace BLL.Interfaces
{
    public interface ICommentService : ICrud<CommentModel>
    {
        Task<IEnumerable<CommentModel>> GetGameComments(Guid gameId);
        Task DeleteAsync(IEnumerable<CommentModel> models);
        Task DeleteAsync(IEnumerable<CommentModel> models, string userId);
    }
}
