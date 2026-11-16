
namespace PlotMusic2.Services;

public interface IImageService
{
    bool IsConfigured { get; }
    Task<string> UploadImageAsync(IFormFile arquivo, ImageFolder pasta);
    Task<bool> DeleteImageAsync(string imageUrl);
}
