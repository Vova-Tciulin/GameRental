namespace GameRental.Domain.Entities;

public class Account
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Login { get; set; }
    public int AccountNumber { get; set; }
    public string Password { get; set; }
    public decimal Price { get; set; }
    public bool IsRented { get; set; } = false;
    public int? ProductId { get; set; } 
    public Product? Product { get; set; }
    public List<Console> Consoles { get; set; } = new();
    public List<Order> Orders { get; set; } = new();

}