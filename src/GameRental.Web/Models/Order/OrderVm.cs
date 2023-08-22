using GameRental.Web.Models.User;

namespace GameRental.Web.Models.Order;

public class OrderVm
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int AccountId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime EndOrderDate { get; set; }
    public decimal Cost { get; set; }
    public bool IsActive { get; set; }
    public UserVm? User { get; set; }
    public AccountVm? Account { get; set; }
}