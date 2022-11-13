using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
{
    public OrderDetailsRepository(GameStoreDbContext db) : base(db) { }

    public async Task<IEnumerable<OrderDetails>> GetOrdersByUserId(Guid id)
    {
        var orders = await db.OrderDetails
                             .Include(god => god.GameOrderDetails)
                             .Where(od => od.UserId == id.ToString())
                             .ToListAsync();
        return orders;
    }
}