using GameRental.Domain.Entities;

namespace GameRental.Domain.IRepository;

public interface IImageRepository
{
    void AddImage(Image image);
    void AddRange(List<Image> images);
    void Remove(Image image);
    Task<IEnumerable<Image>> GetImages(int productId);
}