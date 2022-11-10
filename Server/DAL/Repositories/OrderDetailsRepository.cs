using DAL.Data;
using DAL.Entities;
using DAL.Interfaces;

namespace DAL.Repositories;

public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
{
    public OrderDetailsRepository(GameStoreDbContext db) : base(db)
    {
    }
}