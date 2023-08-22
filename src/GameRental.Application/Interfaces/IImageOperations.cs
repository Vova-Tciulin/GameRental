using Microsoft.AspNetCore.Http;

namespace GameRental.Application.Interfaces;

public interface IImageOperations
{
    Task SaveImagesAsync(params IFormFile[] images);
    void RemoveImages(params string[] imageName);
}