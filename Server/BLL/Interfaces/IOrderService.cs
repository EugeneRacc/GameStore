using BLL.Models;

namespace BLL.Interfaces;

public interface IOrderService
{
    public Task<OrderDetailsModel> AddOrderAsync(OrderDetailsModel model);
    public Task<IEnumerable<OrderDetailsModel>> GetAllOrdersByUserIdAsync(Guid userId);
}