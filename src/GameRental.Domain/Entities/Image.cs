using System.Security.AccessControl;

namespace GameRental.Domain.Entities;

public class Image
{
    public int Id { get; set; }
    public string ImgName { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; } 
}