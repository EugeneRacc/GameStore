using BLL.Enum;

namespace BLL.Models;

public class OrderModel
{
    public Guid? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public PaymentType PaymentType { get; set; }
    public DateTime? OrderDate { get; set; } 
    public string UserId { get; set; }
    public string? Comment { get; set; }
}