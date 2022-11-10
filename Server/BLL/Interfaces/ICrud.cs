using BLL.Models;

namespace BLL.Interfaces
{
    public interface ICrud<TModel> where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel> GetByIdAsync(Guid id);
        Task<TModel> AddAsync(TModel model);
        Task UpdateAsync(TModel model);
        Task DeleteAsync(TModel model);
    }
}
