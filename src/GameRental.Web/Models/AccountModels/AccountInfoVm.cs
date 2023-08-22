using GameRental.Web.Models.Order;
using GameRental.Web.Models.ProductModels;

namespace GameRental.Web.Models.AccountModels;

public class AccountInfoVm
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int AccountNumber { get; set; }
    public bool IsRented { get; set; }
    public int? ProductId { get; set; } 
    public ProductVM? Product { get; set; }
    public List<OrderVm> Orders { get; set; } = new();
    public List<ConsoleVm> Consoles { get; set; } = new();
}