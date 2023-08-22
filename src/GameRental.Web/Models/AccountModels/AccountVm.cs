using GameRental.Web.Models.Order;
using GameRental.Web.Models.ProductModels;

namespace GameRental.Web.Models;

public class AccountVm
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int AccountNumber { get; set; }
    public decimal Price { get; set; }
    public int? ProductId { get; set; }
    public bool IsRented { get; set; }
    public List<OrderVm> Orders { get; set; } = new();
    public ProductVM? Product { get; set; }
    public List<ConsoleVm> Consoles { get; set; } = new();
}