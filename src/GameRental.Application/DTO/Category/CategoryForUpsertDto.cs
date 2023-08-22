using Microsoft.AspNetCore.Http;

namespace GameRental.Application.DTO.Category;

public class CategoryForUpsertDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IFormFile? Image { get; set; }
    
}