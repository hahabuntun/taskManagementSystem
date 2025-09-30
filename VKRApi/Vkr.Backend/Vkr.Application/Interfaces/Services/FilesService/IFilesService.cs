using Microsoft.AspNetCore.Http;
using Vkr.Domain.Enums.Files;
using File = Vkr.Domain.Entities.Files.File;

namespace Vkr.Application.Interfaces.Services.FilesService;

public interface IFilesService
{
    /// <summary>
    /// Загружает файл в S3 и записывает метаданные в БД.
    /// </summary>
    Task<File> UploadAsync(
        FileOwnerType ownerType,
        int ownerId,
        IFormFile formFile,
        string title,
        string? description,
        int userId);

    /// <summary>
    /// Скачивает файл по его Id.
    /// </summary>
    Task<(Stream Stream, string FileName)> DownloadAsync(int fileId);

    /// <summary>
    /// Удаляет запись и сам файл в S3.
    /// </summary>
    Task<bool> DeleteAsync(int fileId, int userId); // Add userId for notification

    /// <summary>
    /// Список файлов для заданного ownerType/ownerId.
    /// </summary>
    Task<IEnumerable<File>> ListByOwnerAsync(FileOwnerType ownerType, int ownerId);
}