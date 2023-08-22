namespace GameRental.Domain.Entities;

public class ProductCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ImgPath { get; set; }
    public List<Product> Products { get; set; } = new();
}