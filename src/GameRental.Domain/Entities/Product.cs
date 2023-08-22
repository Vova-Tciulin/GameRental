namespace GameRental.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int TransitTime { get; set; }
    public string Translate { get; set; }
    public int Year { get; set; }
    public List<ProductCategory> ProductCategories { get; set; } = new();
    public List<Image> Images { get; set; } = new();
    public List<Account> Accounts { get; set; } = new();
}