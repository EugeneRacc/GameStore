namespace DAL.Entities;
public class OrderDetails : BaseEntity
{
    public string UserPhone { get; set; }
    public string PaymentType { get; set; }
    public DateTime OrderDate { get; set; } 
    public string UserId { get; set; }
    public string Comment { get; set; }
    public User User { get; set; }
    public ICollection<GameOrderDetails> GameOrderDetails { get; set; }
    
}