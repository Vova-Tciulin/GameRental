namespace GameRental.Web.Models.ProductModels;

public class ProductAndCategoryVm
{
    public List<ProductVM> ProductVm { get; set; }
    public List<ProductCategoryVM> CategoryVm { get; set; }
    public ProductCategoryVM? Category { get; set; }
}