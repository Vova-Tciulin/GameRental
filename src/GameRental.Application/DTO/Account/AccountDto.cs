using GameRental.Application.DTO.Console;
using GameRental.Application.DTO.Order;
using GameRental.Application.DTO.Product;

namespace GameRental.Application.DTO.Account;

public class AccountDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public int AccountNumber { get; set; }
    public decimal Price { get; set; }
    public bool IsRented { get; set; }
    public int? ProductId { get; set; } 
    public ProductDTO? Product { get; set; }
    public List<ConsoleDto> Consoles { get; set; } = new();
    public List<OrderDto> Orders { get; set; } = new();
}

