namespace DAL.Entities;

public class GameOrderDetails
{
    public Guid GameId { get; set; }
    public Guid OrderDetailsId { get; set; }
    public int Amount { get; set; }
    public Game Game { get; set; }
    public OrderDetails OrderDetails { get; set; }
}