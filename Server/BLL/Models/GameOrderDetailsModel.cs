using DAL.Entities;

namespace BLL.Models
{
    public class GameOrderDetailsModel
    {
        public Guid GameId { get; set; }
        public Guid? OrderDetailsId { get; set; }
        public int Amount { get; set; }
    }
}
