using System.Runtime.InteropServices.JavaScript;

namespace GameRental.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int AccountId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime EndOrderDate { get; set; }
    
    public bool IsActive { get; set; }
    public decimal Cost { get; set; }
    public User? User { get; set; }
    public Account? Account { get; set; }
}