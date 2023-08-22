using GameRental.Domain.Entities;
using GameRental.Domain.IRepository;
using GameRental.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace GameRental.Infrastructure.Repository;

public class ImageRepository:IImageRepository
{
    private readonly GameRentalDbContext _db;

    public ImageRepository(GameRentalDbContext db)
    {
        _db = db;
    }

    public void AddImage(Image image)
    {
        _db.Images.Add(image);
    }

    public void AddRange(List<Image> images)
    {
       _db.Images.AddRange(images);
    }

    public void Remove(Image image)
    {
        _db.Images.Remove(image);
    }

    public async Task<IEnumerable<Image>> GetImages(int productId)
    {
        //return await _db.Images.Where(u => u.ProductId == productId).ToListAsync();
        return null;
    }
}