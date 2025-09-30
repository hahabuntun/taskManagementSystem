using Microsoft.AspNetCore.Http;

namespace Vkr.Application.Interfaces.FileStorage
{
    public interface IFileStorage
    {
        Task<bool> DeleteFileAsync(string fileName);
        Task<Stream> DownloadFileAsync(string fileName);
        Task<(string, long)> UploadFileAsync(IFormFile file, string fileName);
    }
}