using GameRental.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GameRental.Application.SomeLogic;

public class ImageOperations:IImageOperations
{
    private readonly string _imagePath;

    public ImageOperations(string imagePath)
    {
        _imagePath = imagePath+@"\images\";
    }
    

    public async Task SaveImagesAsync(params IFormFile[] images)
    {
        foreach (var image in images)
        {
            await using var fileStream = new FileStream(_imagePath+image.FileName, FileMode.Create);
            await image.CopyToAsync(fileStream);
        }
    }

    public void RemoveImages(params string[] imageName)
    {
        foreach (var image in imageName)
        {
            string path = _imagePath+ image;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}