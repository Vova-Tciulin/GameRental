using GameRental.Web.Models.ProductModels;

namespace GameRental.Web.Models.Cart;

public class CartVm
{
    public int AccountId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int DayOfRent { get; set; }
    public int ProductId { get; set; }
    public string ImgName { get; set; }
}