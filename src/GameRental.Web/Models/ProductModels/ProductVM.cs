using GameRental.Web.Models.AccountModels;

namespace GameRental.Web.Models.ProductModels;

public class ProductVM
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int TransitTime { get; set; }
    public string Translate { get; set; }
    public int Year { get; set; }
    public List<ImageVm> Images { get; set; } = new();
    public List<ProductCategoryVM> ProductCategories { get; set; } = new();
    public List<AccountInfoVm> Accounts { get; set; } = new();
}