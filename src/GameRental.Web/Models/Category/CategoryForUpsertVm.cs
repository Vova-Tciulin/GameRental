namespace GameRental.Web.Models;

public class CategoryForUpsertVm
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IFormFile? Image { get; set; }
    public string? ImgPath { get; set; }
}