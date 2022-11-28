using DAL.Entities;

namespace DAL.Interfaces;

public interface IOrderDetailsRepository : IRepository<OrderDetails>
{
    public Task<IEnumerable<OrderDetails>> GetOrdersByUserId(Guid id);
}